    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _camundaClient;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IOptions<AppSettings> _options;
        private readonly IConsumer<Ignore, string> _consumer;

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
            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_options.Value.KafkaSettings.Topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    _consumer.Commit();
                    _consumer.Close();
                    var data = JsonConvert.DeserializeObject<OperatinData>(consumeResult.Message.Value);
                    if (data != null)
                    {
                        var request = new CompleteRequest() 
                        {
                            BusinessKey = await GenerateBusinessKey()
                        };
                        request.Variables = await request.ConvertObject(data);
                        await StartCamunda(request);
                    }
                    else
                    {
                        _logger.LogError("Message is null: " + JsonConvert.SerializeObject(consumeResult));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"ExecuteAsync error: {ex.Message}");
                }

            }
        }
