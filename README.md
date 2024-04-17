using LegalCashOperationsWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

Вот какой дефолтный мне сгенерил vs

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(new ElasticsearchJsonFormatter(), "logs/.log",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true,
    fileSizeLimitBytes: 10000000)
.CreateLogger();  Конфигурации мне не нужны для логера, достаточно такой логер 
