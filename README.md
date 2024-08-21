{"@timestamp":"2024-08-21T16:25:53.7422299+05:00","level":"Warning","messageTemplate":"The WebRootPath was not found: {WebRootPath}. Static files may be unavailable.","message":"The WebRootPath was not found: \"/app/wwwroot\". Static files may be unavailable.","fields":{"WebRootPath":"/app/wwwroot","EventId":{"Id":16,"Name":"WebRootPathNotFound"},"SourceContext":"Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware"}}
{"@timestamp":"2024-08-21T16:25:53.8277821+05:00","level":"Error","messageTemplate":"Hosting failed to start","message":"Hosting failed to start","exceptions":[{"Depth":0,"ClassName":"System.Net.Sockets.SocketException","Message":"Permission denied","Source":"System.Net.Sockets","StackTraceString":"   at System.Net.Sockets.Socket.DoBind(EndPoint endPointSnapshot, SocketAddress socketAddress)\n   at System.Net.Sockets.Socket.Bind(EndPoint localEP)\n   at Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.SocketTransportOptions.CreateDefaultBoundListenSocket(EndPoint endpoint)\n   at Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.SocketConnectionListener.Bind()\n   at Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.SocketTransportFactory.BindAsync(EndPoint endpoint, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure.TransportManager.BindAsync(EndPoint endPoint, ConnectionDelegate connectionDelegate, EndpointConfig endpointConfig, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl.<>c__DisplayClass28_0`1.<<StartAsync>g__OnBind|0>d.MoveNext()\n--- End of stack trace from previous location ---\n   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.AddressBinder.BindEndpointAsync(ListenOptions endpoint, AddressBindContext context, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.ListenOptions.BindAsync(AddressBindContext context, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.AnyIPListenOptions.BindAsync(AddressBindContext context, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.AddressBinder.AddressesStrategy.BindAsync(AddressBindContext context, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.AddressBinder.BindAsync(ListenOptions[] listenOptions, AddressBindContext context, Func`2 useHttps, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl.BindAsync(CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl.StartAsync[TContext](IHttpApplication`1 application, CancellationToken cancellationToken)\n   at Microsoft.AspNetCore.Hosting.GenericWebHostService.StartAsync(CancellationToken cancellationToken)\n   at Microsoft.Extensions.Hosting.Internal.Host.<StartAsync>b__15_1(IHostedService service, CancellationToken token)\n   at Microsoft.Extensions.Hosting.Internal.Host.ForeachService[T](IEnumerable`1 services, CancellationToken token, Boolean concurrent, Boolean abortOnFirstException, List`1 exceptions, Func`3 operation)","RemoteStackTraceString":null,"RemoteStackIndex":0,"HResult":-2147467259,"HelpURL":null}],"fields":{"EventId":{"Id":11,"Name":"HostedServiceStartupFaulted"},"SourceContext":"Microsoft.Extensions.Hosting.Internal.Host"}}
Unhandled exception. System.Net.Sockets.SocketException (13): Permission denied
   at System.Net.Sockets.Socket.DoBind(EndPoint endPointSnapshot, SocketAddress socketAddress)
   at System.Net.Sockets.Socket.Bind(EndPoint localEP)
   at Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.SocketTransportOptions.CreateDefaultBoundListenSocket(EndPoint endpoint)
   at Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.SocketConnectionListener.Bind()
   at Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.SocketTransportFactory.BindAsync(EndPoint endpoint, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure.TransportManager.BindAsync(EndPoint endPoint, ConnectionDelegate connectionDelegate, EndpointConfig endpointConfig, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl.<>c__DisplayClass28_0`1.<<StartAsync>g__OnBind|0>d.MoveNext()
--- End of stack trace from previous location ---
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.AddressBinder.BindEndpointAsync(ListenOptions endpoint, AddressBindContext context, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.ListenOptions.BindAsync(AddressBindContext context, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.AnyIPListenOptions.BindAsync(AddressBindContext context, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.AddressBinder.AddressesStrategy.BindAsync(AddressBindContext context, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.AddressBinder.BindAsync(ListenOptions[] listenOptions, AddressBindContext context, Func`2 useHttps, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl.BindAsync(CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl.StartAsync[TContext](IHttpApplication`1 application, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Hosting.GenericWebHostService.StartAsync(CancellationToken cancellationToken)
   at Microsoft.Extensions.Hosting.Internal.Host.<StartAsync>b__15_1(IHostedService service, CancellationToken token)
   at Microsoft.Extensions.Hosting.Internal.Host.ForeachService[T](IEnumerable`1 services, CancellationToken token, Boolean concurrent, Boolean abortOnFirstException, List`1 exceptions, Func`3 operation)
   at Microsoft.Extensions.Hosting.Internal.Host.StartAsync(CancellationToken cancellationToken)
   at Microsoft.Extensions.Hosting.HostingAbstractionsHostExtensions.RunAsync(IHost host, CancellationToken token)
   at Microsoft.Extensions.Hosting.HostingAbstractionsHostExtensions.RunAsync(IHost host, CancellationToken token)
   at Microsoft.Extensions.Hosting.HostingAbstractionsHostExtensions.Run(IHost host)
   at Program.<Main>$(String[] args) in /src/Template/Program.cs:line 62

Получаем ошибку при развертывании приложения 

using Serilog.Formatting.Elasticsearch;
using Serilog;
using Template.Extensions.MiddleWares;
using Template.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration() // Настраиваем глобальный логгер проекта Serilog, с выводом в консоль логов, читаем для Еластика формате
    .Enrich.FromLogContext()
    .WriteTo.Console(new ElasticsearchJsonFormatter())
    .CreateLogger();

builder.Host.UseSerilog();

//builder.AddConsulConfiguration(); // Получаем конфигурации проекта из консула, при первичном запуске проекта

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(_ => true));
});
builder.AddServices(); // Внедряем все зависимости проекта
builder.AddDbContexts(); // Внедряем контексты бд
builder.AddKafka(); // Внедряем кафку
builder.AddRedis(); // Внедряем редис

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions // Добавляем entrypoint для получения хелзчека работы приложения, и настраиваем для вывода в едином формате 
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

if (builder.Configuration["SwaggerEnvironment"] == "Development")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>(); // Используем кастомный компонент middleware для аутентификации на основе апи ключа

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();
