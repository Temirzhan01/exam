  "ElasticConfiguration": {
    "Uri": "https://halykbpm-dev-logging.homebank.kz/"
  },

  builder.Services.AddHealthChecks();
ConfigureLogging();
builder.Host.UseSerilog();

void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
        .Enrich.WithProperty("Environment", environment!)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}

    public class TrancheController : ControllerBase
    {
        private readonly ITrancheService _trancheService;
        private readonly ILogger<TrancheController> _logger;

        public TrancheController(ITrancheService trancheService, ILogger<TrancheController> logger)
        {
            _trancheService = trancheService;
            _logger = logger;
        }

        [Route("GetAuthorities/{branchNumber}")]
        [HttpGet]
        public async Task<IActionResult> GetAuthorities(string branchNumber)
        {
            _logger.LogInformation($"GetAuthorities || branchNumber: {branchNumber}");
            if (string.IsNullOrEmpty(branchNumber))
            {
                return BadRequest("Null parametr: branchNumber");
            }
            var result = await _trancheService.GetAuthoritiesAsync(branchNumber);
            if (result.Count() == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }
