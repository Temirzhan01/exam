public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpClient<IMyService, MyService>()
            .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)))
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
}
WaitAndRetryAsync(3, _) — это политика, которая будет пытаться повторить запрос три раза с задержкой в 600 мс между попытками.
CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)) — это политика размыкателя (Circuit Breaker), которая остановит запросы на 30 секунд после 5 подряд неудачных попыток.

public class MyService : IMyService
{
    private readonly HttpClient _httpClient;

    public MyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetDataFromExternalServiceAsync()
    {
        var response = await _httpClient.GetAsync("https://example.com/data");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null; // или обработать ошибку более подробно
    }
}
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpClient<IMyService, MyService>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());
}

private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(response => !response.IsSuccessStatusCode)
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (response, timespan) => 
            {
                // Логирование или действия при размыкании цепи
                Console.WriteLine($"Circuit broken! Break duration: {timespan.TotalSeconds} seconds");
            },
            onReset: () => 
            {
                // Логирование или действия при возврате к нормальной работе
                Console.WriteLine("Circuit reset!");
            },
            onHalfOpen: () => 
            {
                // Логирование или действия при переходе в полуразомкнутое состояние
                Console.WriteLine("Circuit half-open, next call is a trial");
            });
}
public class MyService : IMyService
{
    private readonly HttpClient _httpClient;

    public MyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetDataFromExternalServiceAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://example.com/data");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            
            // Обработка ошибок
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            // Обработка исключений
            Console.WriteLine($"Request failed with exception: {ex.Message}");
            return null;
        }
    }
}


Работа с Redis в .NET 8 может быть эффективно реализована с использованием библиотеки StackExchange.Redis, которая является де-факто стандартом для взаимодействия с Redis в .NET. Давайте рассмотрим основные шаги, которые помогут вам начать работу с Redis в .NET 8.

1. Установка пакета
Первым шагом будет установка пакета StackExchange.Redis через NuGet:

bash
Копировать код
dotnet add package StackExchange.Redis
2. Конфигурация подключения к Redis
После установки пакета необходимо настроить подключение к Redis. Лучше всего это сделать через Dependency Injection (DI), чтобы использовать подключение в сервисах:

csharp
Копировать код
public void ConfigureServices(IServiceCollection services)
{
    // Конфигурация подключения к Redis
    services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
}
Здесь localhost:6379 — это адрес и порт сервера Redis. Вы можете заменить его на нужные вам значения.

3. Использование Redis в сервисах
Теперь, когда подключение настроено, вы можете использовать его в своих сервисах:

csharp
Копировать код
public class MyRedisService
{
    private readonly IConnectionMultiplexer _redis;

    public MyRedisService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<string> GetValueAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetValueAsync(string key, string value)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value);
    }
}
В этом примере GetValueAsync получает значение по ключу из Redis, а SetValueAsync сохраняет значение в Redis по указанному ключу.

4. Регистрация и использование сервиса
Теперь нужно зарегистрировать сервис в DI-контейнере и использовать его:

csharp
Копировать код
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
    services.AddTransient<MyRedisService>();
}

public class MyController : ControllerBase
{
    private readonly MyRedisService _redisService;

    public MyController(MyRedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpGet("get/{key}")]
    public async Task<IActionResult> GetValue(string key)
    {
        var value = await _redisService.GetValueAsync(key);
        return Ok(value);
    }

    [HttpPost("set")]
    public async Task<IActionResult> SetValue([FromQuery] string key, [FromQuery] string value)
    {
        await _redisService.SetValueAsync(key, value);
        return Ok();
    }
}
Здесь MyController использует MyRedisService для работы с Redis, предоставляя API для установки и получения значений.

5. Настройка и оптимизация
Для оптимизации работы с Redis вы можете настроить различные параметры подключения, такие как тайм-ауты, политики повторных попыток и т.д.:

csharp
Копировать код
var options = ConfigurationOptions.Parse("localhost:6379");
options.AbortOnConnectFail = false;
options.ConnectRetry = 3;

services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));
6. Использование Redis Pub/Sub
Redis также поддерживает шаблон "публикация-подписка" (Pub/Sub), который можно использовать для уведомления частей системы о событиях:

csharp
Копировать код
public async Task SubscribeAsync(string channel)
{
    var sub = _redis.GetSubscriber();
    await sub.SubscribeAsync(channel, (ch, message) =>
    {
        Console.WriteLine($"Message received from {ch}: {message}");
    });
}

public async Task PublishAsync(string channel, string message)
{
    var sub = _redis.GetSubscriber();
    await sub.PublishAsync(channel, message);
}
Заключение
Работа с Redis в .NET 8 через библиотеку StackExchange.Redis проста и интуитивно понятна. Следуя этим шагам, вы можете настроить подключение, использовать Redis для хранения данных, реализовать Pub/Sub и даже оптимизировать параметры подключения. Если у вас появятся дополнительные вопросы или задачи, связанные с Redis, не стесняйтесь обращаться!


            builder.Services.AddDbContext<OracleDbContext>(options =>
            {
                var constrBuilder = new OracleConnectionStringBuilder(builder.Configuration.GetConnectionString("OracleConnectionString"));
                options.UseOracle(constrBuilder.ConnectionString, options => options.UseOracleSQLCompatibility()); //тут не принимает строку, только объект класса OracleSQLCompatibility, хотя можно было прописать строкой "11"
            });
