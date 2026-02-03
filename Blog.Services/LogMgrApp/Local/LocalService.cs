using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using System.Reflection;

namespace Blog.Services.Local;

public class LocalService
{
    public static string GetLocalByIp(string ip)
    {
        
        try
        {
            // 使用 Path.Combine 来构建跨平台兼容的路径
            // 注意：文件名是 ip2region_v4.xdb 而不是 ip2region.xdb
            string dbPath = Path.Combine(Environment.CurrentDirectory, "xdb", "ip2region_v4.xdb");
            
            // 检查文件是否存在
            if (!File.Exists(dbPath))
            {
                // 尝试在不同位置查找数据库文件，使用正确的文件名
                var possiblePaths = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xdb", "ip2region_v4.xdb"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "xdb", "ip2region_v4.xdb"),
                    Path.Combine(Directory.GetCurrentDirectory(), "xdb", "ip2region_v4.xdb"),
                    Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? "", "xdb", "ip2region_v4.xdb"),
                    // 也尝试原始文件名作为备选
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xdb", "ip2region.xdb")
                };

                dbPath = possiblePaths.FirstOrDefault(File.Exists) ?? "";
                
                if (string.IsNullOrEmpty(dbPath))
                {
                    return "IP database file not found";
                }
            }
            
            ISearcher searcher = new Searcher(new CachePolicy(), dbPath);
            string? ipInfo = searcher.Search(ip);
            return string.IsNullOrEmpty(ipInfo) ? "unknown" : ipInfo;
        }
        catch (Exception ex)
        {
            // 记录异常但不暴露内部细节
            Console.WriteLine($"IP search error: {ex.Message}"); // 在生产环境中，可以使用更好的日志记录
            return "location lookup failed";
        }
    }
}