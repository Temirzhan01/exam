using Consul;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SPMUtils;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            string reqNumber = (await _soapService.SearchForTransaction(transactionCode)).RequestNumber;
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

        public async Task<string> ChangeStatus(string transactionCode, int status)
        {
            var result = await _soapService.GetTransactionInformation(transactionCode);
            _logger.LogInformation($"Request number: {result.RequestNumber}/Transaction type: {result.TransType}");

            Case OurCase = config.Cases[result.TransType];
            TaskData taskData = await GetTaskData(result.RequestNumber);
            var intermediate = JsonConvert.DeserializeObject<TaskModel>(taskData.taskModel);
            intermediate.status = status;
            taskData.taskModel = JsonConvert.SerializeObject(intermediate);
            taskData.bpRoleCode = OurCase.role;
            taskData.bpVersionId = OurCase.version;
            taskData.resultActionCode = OurCase.action;
            var body = JsonConvert.SerializeObject(taskData);
            _logger.LogInformation($"Model: {body}");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(config.Ensemble);
            using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"finish/task") { Content = new StringContent(body, Encoding.UTF8, "application/json") }))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }

        }
    }
}
