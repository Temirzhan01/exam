using Consul;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TcrService> _logger;

        public EnsembleServiceUrl config
        {
            get
            {
                var configData = Task.Run(async () => await _consulClient.KV.Get("TcrEnsembleServices"));
                return JsonConvert.DeserializeObject<EnsembleServiceUrl>(configData.Result.Response.Value.toString());
            }
        }

        public TcrService(IConsulClient consulClient, IHttpClientFactory httpClientFactory, ILogger<TcrService> logger)
        {
            _consulClient = consulClient;
            _httpClientFactory = httpClientFactory;
            _soapService = new SoapService(config.TcrService, 1000);
            _logger = logger;
        }

        public async Task<TaskModel> GetInfo(string transactionCode)
        {
            string reqNumber = (await _soapService.GetTransaction(transactionCode)).RequestNumber;
            if (!string.IsNullOrEmpty(reqNumber))
            {
                _logger.LogInformation($"Request number: {reqNumber}");
                return JsonConvert.DeserializeObject<TaskModel>((await GetTaskData(reqNumber)).taskModel);
            }
            else
            {
                throw new HttpRequestException("Null request number");
            }
        }

        public async Task<TaskData> GetTaskData(string reqNumber)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(config.Ensemble);
            using (var response = await client.GetAsync($"get/task/{reqNumber}"))
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<TaskData>(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> ChangeStatus(string reqNumber, int status)
        {
            TaskData taskData = await GetTaskData(reqNumber);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(config.Ensemble);
            var intermediate = JsonConvert.DeserializeObject<TaskModel>(taskData.taskModel);
            intermediate.status = status;
            taskData.taskModel = JsonConvert.SerializeObject(intermediate);
            var body = JsonConvert.SerializeObject(taskData);
            using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"finish/task") { Content = new StringContent(body, Encoding.UTF8, "application/json") }))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
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
using TcrService3In1.Extensions;

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
            app.UseMiddleware<ExceptionMiddleware>();
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
