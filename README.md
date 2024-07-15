Для того чтобы при сборке приложения на .NET 6 автоматически подтягивались конфигурационные данные из Consul, можно использовать подход с пользовательскими поставщиками конфигурации. Вот пошаговый пример того, как это можно реализовать:

1. Установка необходимых пакетов
Вам понадобятся следующие NuGet пакеты:

Consul
Microsoft.Extensions.Configuration
Microsoft.Extensions.Configuration.Json
Вы можете установить их через командную строку:

bash
Копировать код
dotnet add package Consul
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
2. Создание кастомного поставщика конфигурации для Consul
Создайте класс ConsulConfigurationProvider, который будет отвечать за получение данных из Consul:

csharp
Копировать код
using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ConsulConfigurationProvider : ConfigurationProvider
{
    private readonly string _consulAddress;
    private readonly string _key;

    public ConsulConfigurationProvider(string consulAddress, string key)
    {
        _consulAddress = consulAddress;
        _key = key;
    }

    public override void Load()
    {
        var client = new ConsulClient(config =>
        {
            config.Address = new Uri(_consulAddress);
        });

        Task<Dictionary<string, string>> getConsulData = GetConsulData(client, _key);
        getConsulData.Wait();

        foreach (var kvp in getConsulData.Result)
        {
            Data.Add(kvp.Key, kvp.Value);
        }
    }

    private async Task<Dictionary<string, string>> GetConsulData(ConsulClient client, string key)
    {
        var result = new Dictionary<string, string>();
        var queryResult = await client.KV.Get(key);

        if (queryResult.Response != null)
        {
            string consulValue = System.Text.Encoding.UTF8.GetString(queryResult.Response.Value);
            result[key] = consulValue;
        }

        return result;
    }
}
3. Создание расширения для IConfigurationBuilder
Теперь создайте расширение, чтобы упростить добавление вашего кастомного поставщика конфигурации в IConfigurationBuilder:

csharp
Копировать код
using Microsoft.Extensions.Configuration;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string consulAddress, string key)
    {
        return builder.Add(new ConsulConfigurationSource(consulAddress, key));
    }
}

public class ConsulConfigurationSource : IConfigurationSource
{
    private readonly string _consulAddress;
    private readonly string _key;

    public ConsulConfigurationSource(string consulAddress, string key)
    {
        _consulAddress = consulAddress;
        _key = key;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new ConsulConfigurationProvider(_consulAddress, _key);
    }
}
4. Настройка Program.cs
В Program.cs добавьте вызов вашего кастомного поставщика конфигурации:

csharp
Копировать код
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();
                var consulAddress = builtConfig["Consul:Address"];
                var consulKey = builtConfig["Consul:Key"];

                config.AddConsul(consulAddress, consulKey);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
5. Конфигурационный файл appsettings.json
Не забудьте добавить начальные настройки Consul в ваш appsettings.json:

json
Копировать код
{
  "Consul": {
    "Address": "http://localhost:8500",
    "Key": "my-application/config"
  }
}
Заключение
Этот подход позволяет вам при сборке и запуске приложения подтягивать конфигурационные данные из Consul и внедрять их в вашу конфигурацию приложения. При необходимости вы можете дополнительно обработать данные, например, преобразовать их в нужные форматы или добавить дополнительные уровни вложенности.


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Consul configuration
var configBuilder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var builtConfig = configBuilder.Build();
var consulAddress = builtConfig["Consul:Address"];
var consulKey = builtConfig["Consul:Key"];

configBuilder.AddConsul(consulAddress, consulKey);

var config = configBuilder.Build();
builder.Configuration.AddConfiguration(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

