using Confluent.Kafka;
using LegalCashOperationsWorker.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace LegalCashOperationsWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _camundaClient;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IOptions<AppSettings> _options;

        public Worker(ILogger<Worker> logger, IHttpClientFactory factory, IOptions<AppSettings> options)
        {
            _options = options;
            _camundaClient = factory.CreateClient("Camunda");
            _logger = logger;
            _consumerConfig = new ConsumerConfig()
            {
                GroupId = options.Value.KafkaSettings.GroupId,
                BootstrapServers = options.Value.KafkaSettings.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = options.Value.KafkaSettings.SaslUsername,
                SaslPassword = options.Value.KafkaSettings.SaslPassword,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableSslCertificateVerification = false,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = true
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                    using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
                    {
                        consumer.Subscribe(_options.Value.KafkaSettings.Topic);
                        var consumeResult = consumer.Consume(stoppingToken);
                        consumer.Commit();
                        consumer.Close();
                        var data = JsonConvert.DeserializeObject<OperatinData>(consumeResult.Message.Value);
                        if (data != null)
                        {
                            var request = new CompleteRequest();
                            request.Variables = await request.ConvertObject(data);
                            request.BusinessKey = await GenerateBusinessKey();
                            await StartCamunda(request);
                        }
                        else
                        {
                            _logger.LogError("Message is null: " + JsonConvert.SerializeObject(consumeResult));
                        }
                    }
                }
                catch (Exception ex) 
                {
                    _logger.LogError($"ExecuteAsync error: {ex.Message}");
                }

            }
        }

        public async Task<string> GenerateBusinessKey()
        {
            try
            {
                var response = await _camundaClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"request-number-generator/requestnumber?reqType={_options.Value.BKGeneratorType}"));
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<ReqNumberResponse>(await response.Content.ReadAsStringAsync()).requestNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Generating business key error: {ex.Message}");
                throw;
            }

        }

        public async Task StartCamunda(CompleteRequest request)
        {
            try
            {
                var response = await _camundaClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"bpm-engine/rest/process-definition/key/{_options.Value.ProcessName}/start") { Content = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }), Encoding.UTF8, "application/json") });
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Starting process error: {ex.Message}");
                throw;
            }
        }
    }
}
