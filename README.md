public class ErrorCounterService
{
    private int _errorCount = 0;
    private readonly int _maxErrors;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);
    private readonly HttpClient _httpClient;
    private bool _isExternalServiceAvailable = true;

    public ErrorCounterService(int maxErrors, HttpClient httpClient)
    {
        _maxErrors = maxErrors;
        _httpClient = httpClient;
        Task.Run(() => CheckExternalServiceAvailability());
    }

    public bool IsThresholdReached => _errorCount >= _maxErrors && !_isExternalServiceAvailable;

    public void IncrementErrorCount()
    {
        Interlocked.Increment(ref _errorCount);
    }

    public void ResetErrorCount()
    {
        Interlocked.Exchange(ref _errorCount, 0);
    }

    private async Task CheckExternalServiceAvailability()
    {
        while (true)
        {
            if (_errorCount >= _maxErrors)
            {
                // Логика проверки доступности внешнего сервиса
                var response = await _httpClient.GetAsync("URL_внешнего_сервиса");
                if (response.IsSuccessStatusCode)
                {
                    _isExternalServiceAvailable = true;
                    ResetErrorCount();
                }
                else
                {
                    _isExternalServiceAvailable = false;
                }
            }
            await Task.Delay(_checkInterval);
        }
    }
}

services.AddSingleton<ErrorCounterService>(new ErrorCounterService(n, new HttpClient()));

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ErrorCounterService errorCounterService)
    {
        if (errorCounterService.IsThresholdReached)
        {
            context.Response.StatusCode = 504;
            await context.Response.WriteAsync("Сервис временно недоступен.");
            return;
        }

        await _next(context);
    }
}

// В `Startup.cs` или `Program.cs`
app.UseMiddleware<ErrorHandlingMiddleware>();

public class ErrorCounterService
{
    private int _errorCount = 0;
    private readonly int _maxErrors;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);
    private readonly HttpClient _httpClient;
    private bool _isExternalServiceAvailable = true;
    private readonly string _serviceEndpoint;

    public ErrorCounterService(int maxErrors, HttpClient httpClient, string serviceEndpoint)
    {
        _maxErrors = maxErrors;
        _httpClient = httpClient;
        _serviceEndpoint = serviceEndpoint;
        Task.Run(() => CheckExternalServiceAvailability());
    }

    public bool IsThresholdReached => _errorCount >= _maxErrors && !_isExternalServiceAvailable;

    public void IncrementErrorCount()
    {
        Interlocked.Increment(ref _errorCount);
    }

    public void ResetErrorCount()
    {
        Interlocked.Exchange(ref _errorCount, 0);
    }

    private async Task CheckExternalServiceAvailability()
    {
        while (true)
        {
            if (_errorCount >= _maxErrors)
            {
                var response = await _httpClient.GetAsync(_serviceEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    _isExternalServiceAvailable = true;
                    ResetErrorCount();
                }
                else
                {
                    _isExternalServiceAvailable = false;
                }
            }
            await Task.Delay(_checkInterval);
        }
    }
}

services.AddSingleton<ErrorCounterService>(serviceProvider =>
{
    return new ErrorCounterService(10, new HttpClient(), "http://example.com/api");
});
services.AddSingleton<ErrorCounterService>(serviceProvider =>
{
    return new ErrorCounterService(10, new HttpClient(), "http://another-example.com/api");
});

public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
{
    // Определение сервиса на основе URL запроса или другого параметра
    var targetService = DetermineService(context.Request.Path);
    var errorCounterService = serviceProvider.GetService<ErrorCounterService>(targetService);

    if (errorCounterService != null && errorCounterService.IsThresholdReached)
    {
        context.Response.StatusCode = 504;
        await context.Response.WriteAsync("Сервис временно недоступен.");
        return;
    }

    await _next(context);
}

public class ErrorCounterServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ErrorCounterServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ErrorCounterService Create(string endpoint)
    {
        // Пример создания с параметрами специфичными для каждого сервиса
        var maxErrors = 5;  // Можно брать из конфигурации
        var httpClient = _serviceProvider.GetRequiredService<HttpClient>();

        return new ErrorCounterService(maxErrors, httpClient, endpoint);
    }
}

services.AddSingleton<ErrorCounterServiceFactory>();
services.AddSingleton<HttpClient>();

public class SomeService
{
    private readonly ErrorCounterService _errorCounterService;

    public SomeService(ErrorCounterServiceFactory factory)
    {
        // Создание сервиса для конкретного ендпоинта
        _errorCounterService = factory.Create("http://example.com/api");
    }

    // Использование _errorCounterService для обработки запросов к этому ендпоинту
}
