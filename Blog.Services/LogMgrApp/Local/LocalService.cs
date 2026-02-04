using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using Microsoft.Extensions.Logging;

namespace Blog.Services.Local;

public class LocalService(ILogger<LocalService> logger)
{
    public string GetLocalByIp(string ip)
    {
        try
        {
            string dbPath = Path.Combine(Environment.CurrentDirectory, "xdb", "ip2region_v4.xdb");
            
            // 检查文件是否存在
            if (!File.Exists(dbPath))
            {
                logger.LogInformation("xdb file not found, please check your installation.");
                return string.Empty;
            }
            
            ISearcher searcher = new Searcher(new CachePolicy(), dbPath);
            string? ipInfo = searcher.Search(ip);
            return ipInfo ?? string.Empty;
        }
        catch (Exception ex)
        {
            // 记录异常但不暴露内部细节
            logger.LogError(ex, "IP search error: {ErrorMessage}", ex.Message);
            return string.Empty;
        }
    }
}