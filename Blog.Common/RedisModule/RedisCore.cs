using Blog.Common.Utils;
using StackExchange.Redis;

namespace Blog.Common.RedisModule;

public class RedisCore
{
    private ConnectionMultiplexer? _connect;

    public IDatabase Db { get; }
    public RedisCore()
    {
        if (AppSettings.Configuration == null)
        {
            throw new Exception("AppSettings.Configuration is null");
        }
        var redisModle = AppSettings.Configuration.GetValidatedConfig<RedisModle>("Redis");
        redisModle.ValidateOrThrow("redis model is null");
        var connectionString = $"{redisModle.Host}:{redisModle.Port},password={redisModle.Password}";
        _connect = ConnectionMultiplexer.Connect(connectionString);
        Db = _connect.GetDatabase();
    }
}