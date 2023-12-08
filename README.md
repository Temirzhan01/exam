namespace TcrServiceMonitoring.Models
{
    public class ErrorRequest
    {
        public string Request_Number { get; set; }
        public string Operation { get; set; }
        public string Currency{ get; set; }
        public string UpdateDate { get; set; }
        public string Initiator { get; set; }
        public string Uname { get; set; }
        public string TcrModel { get; set; }
    }
}
namespace TcrServiceMonitoring.Models
{
    public class SuccessRequest
    {
        public string Request_Number { get; set; }
        public string UpdateDate { get; set; }
        public string ClientName { get; set; }
        public string Operation { get; set; }
        public string Currency{ get; set; }
        public string Initiator { get; set; }
        public string UserName { get; set; }
        public string TcrModel { get; set; }
    }
}
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TcrServiceMonitoring.Models;
using TcrServiceMonitoring.Services;

namespace TcrServiceMonitoring.Controllers
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
        [Route("GetAllErrorRequests/{fromDate}/{toDate}")]
        public async Task<ActionResult<IEnumerable<ErrorRequest>>> GetAllErrorRequests(string fromDate, string toDate)
        {
            Console.WriteLine(DateTime.Now +  " || FromDate: " + fromDate + " || Todate: " + toDate);
            try
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    return await _service.GetAllErrorRequests(fromDate, toDate);
                }
                else
                {
                    return StatusCode(400, "Internal server error: Null parametr(s)");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetAllSuccesRequests/{fromDate}/{toDate}")]
        public async Task<ActionResult<IEnumerable<SuccessRequest>>> GetAllSuccesRequests(string fromDate, string toDate)
        {
            Console.WriteLine(DateTime.Now + " || FromDate: " + fromDate + " || Todate: " + toDate);
            try
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    return await _service.GetAllSuccesRequests(fromDate, toDate);
                }
                else
                {
                    return StatusCode(400, "Internal server error: Null parametr(s)");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error " + e.Message);
            }
        }

    }
}
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using TcrServiceMonitoring.Models;

namespace TcrServiceMonitoring.Repositories
{
    public interface ITcrRepository
    {
        public Task<List<ErrorRequest>> GetAllErrorRequests(OracleCommand command);
        public Task<List<SuccessRequest>> GetAllSuccesRequests(OracleCommand command);
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TcrServiceMonitoring.Models;
using Consul;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SpmModels;
using SPMUtils;
using System.Net.Http;

namespace TcrServiceMonitoring.Repositories
{
    public class TcrRepository : ITcrRepository
    {
        private readonly IConsulClient _consulClient;
        private readonly IConfiguration _configuration;

        public TcrRepository(IConsulClient consulClient, IConfiguration configuration) 
        {
            _configuration = configuration;
            _consulClient = consulClient;
        }

        public ConnectionStringToBase config
        {
            get
            {
                var configData = Task.Run(async () => await _consulClient.KV.Get(_configuration["Environment:ConsulKvName"]));
                return JsonConvert.DeserializeObject<ConnectionStringToBase>(configData.Result.Response.Value.toString());
            }
        }

        public async Task<List<ErrorRequest>> GetAllErrorRequests(OracleCommand command) 
        {
            var result = new List<ErrorRequest>();
            using (OracleConnection conn = new OracleConnection(config.CARDCOLV))
            {
                try
                {
                    await conn.OpenAsync();
                    OracleCommand cmd = command;
                    cmd.Connection = conn;
                    OracleDataReader reader = (OracleDataReader)await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result.Add(new ErrorRequest()
                        {
                            Request_Number = reader["REQUEST_NUMBER"].ToString(),
                            UpdateDate = reader["UPDATE_DATE"].ToString(),
                            Operation = reader["OPERATION"].ToString(),
                            Currency = reader["CURRENCY"].ToString(),
                            Initiator = reader["INITIATOR"].ToString(),
                            Uname = reader["UNAME"].ToString(),
                            TcrModel = reader["TCRMODEL"].ToString()
                        });
                    }
                    reader.Dispose();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw new HttpRequestException(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public async Task<List<SuccessRequest>> GetAllSuccesRequests(OracleCommand command)
        {
            var result = new List<SuccessRequest>();
            using (OracleConnection conn = new OracleConnection(config.CARDCOLV))
            {
                try
                {
                    await conn.OpenAsync();
                    OracleCommand cmd = command;
                    cmd.Connection = conn;
                    OracleDataReader reader = (OracleDataReader)await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result.Add(new SuccessRequest()
                        {
                            Request_Number = reader["REQUEST_NUMBER"].ToString(),
                            UpdateDate = reader["UPDATE_DATE"].ToString(),
                            ClientName = reader["CLIENTNAME"].ToString(),
                            Operation = reader["OPERATION"].ToString(),
                            Currency = reader["CURRENCY"].ToString(),
                            Initiator = reader["INITIATOR"].ToString(),
                            UserName = reader["USERNAME"].ToString(),
                            TcrModel = reader["TCRMODEL"].ToString()
                        });
                    }
                    reader.Dispose();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw new HttpRequestException(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using TcrServiceMonitoring.Models;

namespace TcrServiceMonitoring.Services
{
    public interface ITcrService
    {
        public Task<List<ErrorRequest>> GetAllErrorRequests(string fromDate, string toDate);
        public Task<List<SuccessRequest>> GetAllSuccesRequests(string fromDate, string toDate);
    }
}
using System.Collections.Generic;
using TcrServiceMonitoring.Models;
using TcrServiceMonitoring.Repositories;
using System;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace TcrServiceMonitoring.Services
{
    public class TcrService : ITcrService
    {
        private readonly ITcrRepository _repository;
        public TcrService(ITcrRepository repository) 
        {
            _repository = repository;
        }

        public async Task<List<ErrorRequest>> GetAllErrorRequests(string fromDate, string toDate) 
        {
            if (IsValid(fromDate, toDate))
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "cardcolv.PKG_TCR.GETALLERRORREQUESTS";
                cmd.Parameters.Add("FROMDATE", OracleDbType.NVarchar2, fromDate, ParameterDirection.Input);
                cmd.Parameters.Add("TODATE", OracleDbType.NVarchar2, toDate, ParameterDirection.Input);
                cmd.Parameters.Add("CUR_OUT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                List<ErrorRequest> errors = await _repository.GetAllErrorRequests(cmd);
                if (errors.Count > 0)
                {
                    return errors;
                }
                else
                {
                    throw new HttpRequestException("No data found at this period");
                }
            }
            else
            {
                throw new HttpRequestException("Parametrs not valid");
            }
        }

        public async Task<List<SuccessRequest>> GetAllSuccesRequests(string fromDate, string toDate)
        {
            if (IsValid(fromDate, toDate))
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "cardcolv.PKG_TCR.GETALLSUCCESSREQUESTS";
                cmd.Parameters.Add("FROMDATE", OracleDbType.Varchar2, fromDate, ParameterDirection.Input);
                cmd.Parameters.Add("TODATE", OracleDbType.Varchar2, toDate, ParameterDirection.Input);
                cmd.Parameters.Add("CUR_OUT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                List<SuccessRequest> successes = await _repository.GetAllSuccesRequests(cmd);
                if (successes.Count > 0)
                {
                    return successes;
                }
                else
                {
                    throw new HttpRequestException("No data found at this period");
                }
            }
            else
            {
                throw new HttpRequestException("Parametrs not valid");
            }
        }
        public bool IsValid(string fromDate, string toDate)
        {
            if (fromDate.Length == 19 || toDate.Length == 19)
            {
                CultureInfo culture = new CultureInfo("ru-RU");
                culture.Calendar.TwoDigitYearMax = 2099;
                var dateFormat = "dd.MM.yyyy HH:mm:ss";
                DateTime scheduleDate;
                if (DateTime.TryParseExact(fromDate, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate) && DateTime.TryParseExact(toDate, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate))
                {
                    if ((DateTime.ParseExact(toDate, dateFormat, culture) - DateTime.ParseExact(fromDate, dateFormat, culture)).TotalHours <= 24)
                    {
                        return true;
                    }
                    else 
                    {
                        throw new HttpRequestException("Parametrs not valid: More than 24 hours");
                    }
                }
            }
            else if (fromDate == toDate)
            {
                return false;
            }
            return false;
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
using TcrServiceMonitoring.Repositories;
using TcrServiceMonitoring.Services;
using Microsoft.Extensions.Hosting;

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
            services.AddScoped<ITcrRepository, TcrRepository>();
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
