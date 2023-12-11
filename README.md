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
