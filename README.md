using Consul;
using Newtonsoft.Json;
using SPMUtils;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TcrService3In1.Models;
using TcrService3In1.ServiceReference;

namespace TcrService3In1.Services
{
    public class TcrService : ITcrService
    {
        private readonly IConsulClient _consulClient;
        private readonly SoapService _soapService;
        private readonly IHttpClientFactory _httpClientFactory;
        public EnsembleServiceUrl config
        {
            get
            {
                var configData = Task.Run(async () => await _consulClient.KV.Get("TcrEnsembleServices"));
                return JsonConvert.DeserializeObject<EnsembleServiceUrl>(configData.Result.Response.Value.toString());
            }
        }
        public TcrService(IConsulClient consulClient, IHttpClientFactory httpClientFactory) 
        {
            _consulClient = consulClient;
            _httpClientFactory = httpClientFactory;
            _soapService = new SoapService(config.TcrService, 1000);
        }

        public async Task<TaskModel> GetInfo(string transactionCode)
        {
            try
            {
                string reqNumber = (await _soapService.GetTransaction(transactionCode)).RequestNumber; 
                if (!string.IsNullOrEmpty(reqNumber))
                {
                    Console.WriteLine($"Request number: {reqNumber}");
                    return JsonConvert.DeserializeObject<TaskModel>((await GetTaskData(reqNumber)).taskModel);
                }
                else
                {
                    throw new HttpRequestException("Null request number");
                }

            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task<string> ChangeStatus(string reqNumber, int status)
        {
            try
            {
                TaskData taskData = await GetTaskData(reqNumber);
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(config.Ensemble);
                var intermediate = JsonConvert.DeserializeObject<TaskModel>(taskData.taskModel);
                intermediate.status = status;
                taskData.taskModel = JsonConvert.SerializeObject(intermediate);
                var body = JsonConvert.SerializeObject(taskData);
                using (HttpResponseMessage response = await client.SendAsync( new HttpRequestMessage(HttpMethod.Post, $"finish/task") { Content = new StringContent(body, Encoding.UTF8, "application/json") }))
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task<TaskData> GetTaskData(string reqNumber)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(config.Ensemble);
                using (var response = await client.GetAsync($"get/task/{reqNumber}"))
                {
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<TaskData>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TcrService3In1.Models;
using TcrService3In1.Services;

namespace TcrService3In1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class TcrController : ControllerBase
    {
        private readonly ITcrService _service;
        public TcrController(ITcrService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetInfo/{transactionCode}")]
        public async Task<ActionResult<TaskModel>> GetInfo(string transactionCode)
        {
            Console.WriteLine(DateTime.Now + " || GetInfo/Transaction Code: " + transactionCode);
            try
            {
                if (!string.IsNullOrEmpty(transactionCode))
                {
                    return await _service.GetInfo(transactionCode);
                }
                else
                {
                    return StatusCode(400, "Internal server error: Null parametr");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error " + e.Message);
            }
        }

        [HttpPost]
        [Route("ChangeStatus/{reqNumber}/{status}")]
        public async Task<ActionResult<string>> ChangeStatus(string reqNumber, int status)
        {
            Console.WriteLine(DateTime.Now + " || ChangeStatus/Request Number: " + reqNumber + " || Status: " + status);
            try
            {
                if (!string.IsNullOrEmpty(reqNumber) && status != null)
                {
                    return await _service.ChangeStatus(reqNumber, status);
                }
                else
                {
                    return StatusCode(400, "Internal server error: Null parametr");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error " + e.Message);
            }
        }
    }
}
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;

namespace TcrService3In1.ServiceReference
{
    public class SoapService
    {
        private readonly TcrServiceSoap serviceSoap;
        public SoapService(string endpointAddres, double timeout) 
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                SendTimeout = TimeSpan.FromSeconds(timeout),
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max,
            };

            EndpointAddress address = new EndpointAddress(endpointAddres);
            ChannelFactory<TcrServiceSoapChannel> factory = new ChannelFactory<TcrServiceSoapChannel>(binding, address);
            this.serviceSoap = factory.CreateChannel();
        }

        public async Task<TransactionInformation> GetTransaction(string transactionCode) 
        {
            return await this.serviceSoap.SearchForTransactionAsync(transactionCode);
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JenkinsServiceTools.Extensions;
using SpmModels;
using TcrService3In1.Services;
using Microsoft.Extensions.Hosting;
using TcrService3In1.ServiceReference;

namespace JenkinsServiceTools
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(_ => true));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            services.Configure<SoapProxyConfigs>(Configuration);
            services.AddConsul(Configuration);
            services.AddScoped<ITcrService, TcrService>();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JenkinsServiceTools.Extensions
{
    public static class Consul
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                //consul address  
                var address = configuration["Consul:Host"];
                consulConfig.Address = new Uri(address);
            }, null, handlerOverride =>
            {
                //disable proxy of httpclienthandler  
                handlerOverride.Proxy = null;
                handlerOverride.UseProxy = false;
            }));
            return services;
        }
    }
}
using System.Collections.Generic;

namespace TcrService3In1.Models
{
    public class EnsembleServiceUrl
    {
        public string Ensemble { get; set; }
        public string TcrService { get; set; }
        public Case WO { get; set; }
        public Case DO { get; set; }
    }
    public class Case
    {
        public string role { get; set; }
        public string version { get; set; }
        public string action { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace TcrService3In1.Models
{
    public class TaskData
    {
        public string taskModel { get; set; }
        public string taskId { get; set; }
        public string processId { get; set; }
        public string resultActionCode { get; set; }
        public string bpVersionId { get; set; }
        public string bpRoleCode { get; set; }
    } 

    public class TaskModel
    {
        public int processType { get; set; }
        public BankEmployee bankEmployee { get; set; }
        public bool isOnlineBank { get; set; }
        public bool isHomeBank { get; set; }
        public string homebankId { get; set; }
        public bool isConfidant { get; set; }
        public string cassOrder { get; set; }
        public bool cardNotFound { get; set; }
        public bool tcrGetRedy { get; set; }
        public Branch branch { get; set; }
        public bool showTextWarn { get; set; }
        public bool tcrGo { get; set; }
        public bool showTcrErr { get; set; }
        public bool returned { get; set; }
        public GetStatRes getStatRes { get; set; }
        public string errorCalculate { get; set; }
        public string requestNumber { get; set; }
        public DateTime requestCreateDate { get; set; }
        public ClientInfo clientInfo { get; set; }
        public Recycler recycler { get; set; }
        public WithdrawRes withdrawRes { get; set; }
        public string rrko { get; set; }
        public DateTime rrkodate { get; set; }
        public Transaction transaction { get; set; }
        public string summAmount { get; set; }
        public SetAccount setAccount { get; set; }
        public SetExtAccount setExtAccount { get; set; }
        public string commision { get; set; }
        public string totalComm { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string iin { get; set; }
        public string documentNumber { get; set; }
        public DateTime documentDate { get; set; }
        public string documentDateString { get; set; }
        public string telephoneNumber { get; set; }
        public string applicantAddress { get; set; }
        public string maxSumCashIn { get; set; }
        public string warrantNumber { get; set; }
        public int status { get; set; }
        public string statusString { get; set; }
        public List<ExecutionHistory> executionHistory { get; set; }
        public DateTime documenExpiretDate { get; set; }
        public string documentExpireDateString { get; set; }
        public ExecutionResponse executionResponse { get; set; }
        public AuthResponse authResponse { get; set; }
        public FinResponse finResponse { get; set; }
        public string executionId { get; set; }
        public ConfidantInfo confidantInfo { get; set; }
        public string userCassInfo { get; set; }
        public int timeoutMinutes { get; set; }
        public string rkodateString { get; set; }
    }

    public class AuthResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

    public class BankEmployee
    {
        public string login { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string tabNumber { get; set; }
        public string branch { get; set; }
        public string tUserId { get; set; }
        public string eMail { get; set; }
        public List<string> roles { get; set; }
        public string addInfo { get; set; }
        public string fullName { get; set; }
        public string adLogin { get; set; }
    }

    public class Branch
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string p_number { get; set; }
    }

    public class ClientInfo
    {
        public string clientCode { get; set; }
        public string clientId { get; set; }
        public string iinBin { get; set; }
        public string clientName { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string documentIssuer { get; set; }
        public string documentIssueDate { get; set; }
        public bool isResident { get; set; }
        public string birthDate { get; set; }
        public string address { get; set; }
        public string citizenshipCountry { get; set; }
        public string searchResult { get; set; }
        public string firstNameHead { get; set; }
        public string lastNameHead { get; set; }
        public string middleNameHead { get; set; }
        public string iinHead { get; set; }
        public string kod { get; set; }
        public bool isClientCardExists { get; set; }
        public bool isSearchDone { get; set; }
        public string residentCountryCode { get; set; }
    }

    public class ConfidantInfo
    {
        public string id { get; set; }
        public string iin { get; set; }
        public string fullName { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string documentIssuer { get; set; }
        public string documentIssueDate { get; set; }
        public string powerOfAttorneyNumber { get; set; }
        public string powerOfAttorneyIssueDate { get; set; }
        public string birthDate { get; set; }
        public string address { get; set; }
        public string citizenshipCountry { get; set; }
    }

    public class ExecutionHistory
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class ExecutionResponse
    {
        public string resultCode { get; set; }
        public string resultStatus { get; set; }
        public string processId { get; set; }
    }

    public class FinResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

    public class GetStatRes
    {
        public int resultCode { get; set; }
        public string errorText { get; set; }
    }

    public class Kbe
    {
        public string code { get; set; }
        public string displayName { get; set; }
    }

    public class Knp
    {
        public string code { get; set; }
        public string displayName { get; set; }
    }

    public class Recycler
    {
        public string recCode { get; set; }
        public string recName { get; set; }
        public string recLocation { get; set; }
        public string recStatus { get; set; }
        public string recType { get; set; }
        public string kztAmount { get; set; }
        public string usdAmount { get; set; }
        public string eurAmount { get; set; }
        public string depositoryCode { get; set; }
        public string depositoryName { get; set; }
    }

    public class SetAccount
    {
        public string iban { get; set; }
        public string currency { get; set; }
        public string currencyName { get; set; }
        public string amount { get; set; }
        public string depCode { get; set; }
        public string depId { get; set; }
        public string accountSearchResult { get; set; }
        public object status { get; set; }
        public string type { get; set; }
    }

    public class SetExtAccount
    {
        public string iban { get; set; }
        public string currency { get; set; }
        public string currencyName { get; set; }
        public string amount { get; set; }
        public string depCode { get; set; }
        public string depId { get; set; }
        public string accountSearchResult { get; set; }
        public object status { get; set; }
        public string type { get; set; }
    }

    public class Skp
    {
        public string code { get; set; }
        public string displayName { get; set; }
    }

    public class Transaction
    {
        public string account { get; set; }
        public string accountId { get; set; }
        public string accountDepartmentId { get; set; }
        public string accountAmount { get; set; }
        public string accountAviableAmount { get; set; }
        public string accountCurrency { get; set; }
        public string accountType { get; set; }
        public string cardId { get; set; }
        public string excess { get; set; }
        public string analytics { get; set; }
        public bool settlement { get; set; }
        public string transactionAmount { get; set; }
        public string transactionActualAmount { get; set; }
        public string transactionCommission { get; set; }
        public string sessionId { get; set; }
        public string tcrTransactionId { get; set; }
        public string recyclerId { get; set; }
        public string recyclerName { get; set; }
        public string recyclerCity { get; set; }
        public string invoiceAmount { get; set; }
        public string eknp { get; set; }
        public DateTime operDate { get; set; }
        public string operId { get; set; }
        public string reqID { get; set; }
        public string key { get; set; }
        public string reqNum { get; set; }
        public string kod { get; set; }
        public Kbe kbe { get; set; }
        public Knp knp { get; set; }
        public Skp skp { get; set; }
        public string destination { get; set; }
        public string branch { get; set; }
        public string differenceSumm { get; set; }
        public bool difference { get; set; }
        public string differenceType { get; set; }
        public int clientType { get; set; }
    }

    public class WithdrawRes
    {
        public string errorCode { get; set; }
        public string errorText { get; set; }
        public string transactionId { get; set; }
        public string reqNum { get; set; }
        public string recAcc { get; set; }
        public string recAccExt { get; set; }
        public string vmix { get; set; }
        public string sesId { get; set; }
        public string recordId { get; set; }
    }
}
