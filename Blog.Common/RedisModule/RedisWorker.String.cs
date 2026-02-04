namespace Blog.Common.RedisModule;

public partial class RedisWorker(RedisCore redis) : IRedisWorker
{
    public void SetString(string key, string value, TimeSpan expiry = default)
    {
        if (expiry == default(TimeSpan))
            expiry = TimeSpan.FromMinutes(1);
        
        redis.Db.StringSet(key, value, expiry);
    }

    public async Task SetStringAsync(string key, string value, TimeSpan expiry = default)
    {
        if (expiry == default(TimeSpan))
            expiry = TimeSpan.FromMinutes(1);
        await redis.Db.StringSetAsync(key, value, expiry);
    }

    public string? GetString(string key)
    {
        return redis.Db.StringGet(key);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        return await redis.Db.StringGetAsync(key);
    }
}