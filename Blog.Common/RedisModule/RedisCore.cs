using Blog.Common.Utils;
using Blog.Extensions.Validation;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Blog.Common.RedisModule;

public class RedisCore
{
    private ConnectionMultiplexer? _connect;

    public IDatabase Db { get; }
    public RedisCore(ILogger<RedisCore> logger)
    {
        if (AppSettings.Configuration == null)
        {
            throw new Exception("AppSettings.Configuration is null");
        }
        var redisModle = AppSettings.Configuration.GetValidatedConfig<RedisModle>("Redis");
        redisModle.ValidateOrThrow("redis model is null");
        var connectionString = $"{redisModle.Host}:{redisModle.Port},password={redisModle.Password}";
        try
        {
            _connect = ConnectionMultiplexer.Connect(connectionString);
            Db = _connect.GetDatabase();

        }
        catch (RedisException ex)
        {
           logger.LogError($"Redis connect error: {ex.Message}");
        
        }
    }
}