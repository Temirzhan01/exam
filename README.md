using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using System;

namespace YourNamespace
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddExternalServiceHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            // Получаем секцию с внешними сервисами как словарь
            var externalServices = configuration.GetSection("ExternalServices").Get<Dictionary<string, string>>();
            foreach (var service in externalServices)
            {
                string name = service.Key; // Название сервиса, например "HHDSoapService"
                string url = service.Value; // URL сервиса

                if (!string.IsNullOrWhiteSpace(url))
                {
                    // Добавляем health check для каждого URL
                    services.AddHealthChecks().AddUrlGroup(new Uri(url), name: name);
                }
            }

            return services;
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// Добавление health checks для внешних сервисов
builder.Services.AddExternalServiceHealthChecks(builder.Configuration);

var app = builder.Build();

// Конфигурация маршрута для health checks
app.MapHealthChecks("/health", new HealthCheckOptions {
    ResponseWriter = async (context, report) => {
        context.Response.ContentType = "application/json";
        var result = new {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new { name = x.Key, response = x.Value.Status.ToString(), description = x.Value.Description })
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
});

app.Run();

