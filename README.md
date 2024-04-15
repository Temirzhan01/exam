public class AdvancedCircuitBreaker
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private DateTime _lastTrialRequestTime = DateTime.MinValue;

    public AdvancedCircuitBreaker()
    {
        _circuitBreakerPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(30),
                onBreak: (result, breakDelay, context) =>
                {
                    Console.WriteLine($"Circuit broken: {result.Result.StatusCode}");
                },
                onReset: context => Console.WriteLine("Circuit reset."),
                onHalfOpen: () => Console.WriteLine("Circuit is half-open. Trying a single request..."));
    }

    public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action)
    {
        if (_circuitBreakerPolicy.CircuitState == CircuitState.Open &&
            (DateTime.UtcNow - _lastTrialRequestTime).TotalSeconds > 20)
        {
            // Allow a single trial request to go through
            _lastTrialRequestTime = DateTime.UtcNow;
            try
            {
                var response = await action();
                if (response.IsSuccessStatusCode)
                {
                    _circuitBreakerPolicy.Reset(); // Manually resetting the circuit breaker if the external service is available
                }
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.GatewayTimeout); // Return 504 if the trial request fails
            }
        }
        else
        {
            try
            {
                // Regular request processing
                return await _circuitBreakerPolicy.ExecuteAsync(action);
            }
            catch (BrokenCircuitException)
            {
                return new HttpResponseMessage(HttpStatusCode.GatewayTimeout); // Return 504 if the circuit is open
            }
        }
    }
}

public class MyService
{
    private readonly AdvancedCircuitBreaker _externalServiceACircuitBreaker;
    private readonly AdvancedCircuitBreaker _externalServiceBCircuitBreaker;
    private HttpClient _httpClient;

    public MyService()
    {
        _externalServiceACircuitBreaker = new AdvancedCircuitBreaker();
        _externalServiceBCircuitBreaker = new AdvancedCircuitBreaker();
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> CallExternalServiceA(string url)
    {
        return await _externalServiceACircuitBreaker.ExecuteAsync(() => _httpClient.GetAsync(url));
    }

    public async Task<HttpResponseMessage> CallExternalServiceB(string url)
    {
        return await _externalServiceBCircuitBreaker.ExecuteAsync(() => _httpClient.GetAsync(url));
    }
}
