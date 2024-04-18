using LegalCashOperationsWorker;
using LegalCashOperationsWorker.Models;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Elasticsearch;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(new ElasticsearchJsonFormatter(), "logs/.log",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true,
    fileSizeLimitBytes: 10000000)
.CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.Configure<ExternalServices>(configuration.GetSection("ExternalServices"));
        services.AddHttpClient("Camunda", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<ExternalServices>>().Value.Camunda);
        });
    })
    .Build();

await host.RunAsync();
