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

{
  "Redis": {
    "ConnectionString": "localhost:6379,abortConnect=false,ssl=true,password=your_secure_password"
  }
}
