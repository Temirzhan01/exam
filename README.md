[14:33:54 INF] Hosting environment: Development

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using SPM3._0Service.Extensions;
using SPM3._0Service.Extensions.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(new ElasticsearchJsonFormatter(), "logs/.log",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true,
    fileSizeLimitBytes: 10000000)
.CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = async (context, report) => {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new { name = x.Key, response = x.Value.Status.ToString(), description = x.Value.Description })
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
