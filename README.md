using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using SPM3._0Service.Data;
using SPM3._0Service.Extensions;
using SPM3._0Service.Models;
using SPM3._0Service.Repositories;
using SPM3._0Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.Configure<ExternalServices>(builder.Configuration.GetSection("ExternalServices"));
builder.Services.AddDbContext<OracleDbContext>(options =>
{
    var conStrBuilder = new OracleConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseOracle(conStrBuilder.ConnectionString, options => options.UseOracleSQLCompatibility("11"));
});

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

var app = builder.Build();

app.MapHealthChecks("/healthz");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
Оцени этот файл, расскажи конфигурации логирования, они правильно настроены? их возможно вынести в другой файл
