using Confluent.Kafka;
using LegalCashOperationsWorker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace LegalCashOperationsWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _camundaClient;
        private readonly ConsumerConfig _consumerConfig;
        private readonly string _topic;

        public Worker(ILogger<Worker> logger, IHttpClientFactory factory, IOptions<KafkaSettings> options, string topic)
        {
            _camundaClient = factory.CreateClient("Camunda");
            _logger = logger;
            _consumerConfig = new ConsumerConfig()
            {
                GroupId = options.Value.GroupId,
                BootstrapServers = options.Value.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = options.Value.SaslUsername,
                SaslPassword = options.Value.SaslPassword,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableSslCertificateVerification = false,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = true
            };
            _topic = topic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(_topic);
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    _logger.LogInformation(consumeResult.Message.Value);
                    var data = JsonConvert.DeserializeObject<OperatinData>(consumeResult.Message.Value);
                    if (data != null)
                    {
                        await StartProcess(data);
                    }
                }
            }
        }

        public async Task StartProcess(OperatinData operatinData) 
        {
            using (_camundaClient) 
            {
                var response = await _camundaClient.SendAsync(new HttpRequestMessage() { Method = HttpMethod.Post, Content = new StringContent(JsonConvert.SerializeObject(operatinData), Encoding.UTF8, "application/json" ) });
                response.EnsureSuccessStatusCode();
            }
        }

    }
}
