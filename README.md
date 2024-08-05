
public void ConfigureServices(IServiceCollection services)
{
    var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"));
    services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        return ConnectionMultiplexer.Connect(configuration);
    });

    services.AddTransient<IRedisService, RedisService>();
    services.AddControllers();
}

public interface IRedisService
{
    Task<string> GetValueAsync(string key);
    Task SetValueAsync(string key, string value);
    Task<bool> DeleteKeyAsync(string key);
}

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string> GetValueAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetValueAsync(string key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public async Task<bool> DeleteKeyAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }
}

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly AsyncRetryPolicy _retryPolicy;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _retryPolicy = Policy.Handle<RedisConnectionException>()
                             .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2));
    }

    public async Task<string> GetValueAsync(string key)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        });
    }

    // Другие методы остаются без изменений
}
