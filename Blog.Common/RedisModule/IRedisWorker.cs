namespace Blog.Common;

public interface IRedisWorker: ITag
{
    string? GetString(string key);
    Task<string?> GetStringAsync(string key);
    void SetString(string key, string value, TimeSpan expiry = default );
    Task SetStringAsync(string key, string value, TimeSpan expiry = default);
}