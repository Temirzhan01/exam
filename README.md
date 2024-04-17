protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    var config = new ConsumerConfig { BootstrapServers = _kafkaSettings.BootstrapServers, GroupId = _kafkaSettings.GroupId };
    using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
    {
        consumer.Subscribe(_kafkaSettings.Topic);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);
            // Обработка сообщения
        }
    }
}
