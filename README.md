using Confluent.Kafka;
using LegalCashOperationsWorker.Models;
using Microsoft.Extensions.Options;

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
                }
            }
        }

    }
}

Severity	Code	Description	Project	File	Line	Suppression State
Error	CS0051	Inconsistent accessibility: parameter type 'IOptions<KafkaSettings>' is less accessible than method 'Worker.Worker(ILogger<Worker>, IHttpClientFactory, IOptions<KafkaSettings>, string)'	LegalCashOperationsWorker	D:\source\repos\CustomServices\LegalCashOperationsWorker\Worker.cs	14	Active
Получаю такую оишбку? что делать:
