Смотри тут есть у меня класс для внешних сервисов, их может быть намного больше
    public class ExternalServices
    {
        public string Colvir { get; set; }
        public string HHDSoapService { get; set; }
    }
  "ExternalServices": {
    "HHDSoapService": "",
    "Colvir": ""
  },
Я хочу добавить хелзчек, типа такого 
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SPM3._0Service.Data;

namespace SPM3._0Service.Extensions.HealthChecks
{
    public class OracleDbHealthCheck : IHealthCheck
    {
        private readonly OracleDbContext _context;

        public OracleDbHealthCheck(OracleDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                bool status = await _context.Database.CanConnectAsync(cancellationToken);
                if (status)
                {
                    return HealthCheckResult.Healthy("Database connection is OK");
                }
                else return HealthCheckResult.Unhealthy("Database connection failed");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database check failed", ex);
            }
        }
    }
}

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            errors = report.Entries.Select(x => new { key = x.Key, value = x.Value.Description ?? x.Value.Exception?.Message }).Where(x => x.value != null)
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});

Или такого 
using Core6ApiTemplate.Api.HealthChecks;

namespace Core6ApiTemplate.Api.Extensions
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            foreach (var uri in configuration.GetSection("Uris").AsEnumerable(makePathsRelative: true))
            {
                services.AddHealthChecks().AddUrlGroup(new Uri(uri.Value ?? ""), name: uri.Key);
            }

            services.AddHealthChecks()
                .AddOracle(configuration.GetConnectionString("spm") ?? "", name: "spm")
                .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck));

            // AspNetCore.HealthChecks.Consul       6.0.2   .AddConsul()
            // AspNetCore.HealthChecks.SqlServer    6.0.2   .AddSQLServer()
            // AspNetCore.HealthChecks.Npgsql       6.0.2   .AddNpgSql()
            // AspNetCore.HealthChecks.Kafka        6.0.2   .AddKafka()
            // AspNetCore.HealthChecks.Redis        6.0.2   .AddRedis()
            // AspNetCore.HealthChecks.RabbitMQ     6.0.2   .AddRabbitMQ()

            return services;
        }
    }
}


И хочу чтобы, мой хелзчек проходился по всем юрлам и проверял на доступность.
Какой лучше всего? 
