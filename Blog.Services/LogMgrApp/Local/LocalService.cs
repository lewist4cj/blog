using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;

namespace Blog.Services.Local;

public class LocalService
{
    public static string GetLocalByIp(string ip)
    {
        try
        {
            ISearcher searcher = new Searcher(new CachePolicy(), Environment.CurrentDirectory + @"\\xdb\\ip2region.xdb");
            string? ipInfo = searcher.Search(ip);
            return string.IsNullOrEmpty(ipInfo) ? "unknow: " : ipInfo;
        }
        catch (Exception ex)
        {
            return $"IP search failed: {ex.Message}";
        }
    }
}