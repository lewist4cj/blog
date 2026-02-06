using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Blog.Common;

/// <summary>
///     appsettings.json操作类
/// </summary>
public class AppSettings
{
    public AppSettings(string contentPath)
    {
        // var Path = "appsettings.json";

        //如果你把配置文件 是 根据环境变量来分开了，可以这样写
        var Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

        Configuration = new ConfigurationBuilder()
            .SetBasePath(contentPath)
            .Add(new JsonConfigurationSource
            {
                Path = Path,
                Optional = false,
                ReloadOnChange = true
            }) //这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
            .Build();
    }

    public AppSettings(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static IConfiguration? Configuration { get; set; }
    private static string contentPath { get; set; }

    /// <summary>
    ///     封装要操作的字符
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static string? app(params string[] sections)
    {
        try
        {
            if (sections.Length != 0) return Configuration?[string.Join(":", sections)];
        }
        catch (Exception)
        {
            // ignored
        }

        return "";
    }

    /// <summary>
    ///     递归获取配置信息数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sections"></param>
    /// <returns></returns>
    public static List<T> app<T>(params string[] sections)
    {
        var list = new List<T>();
        // 引用 Microsoft.Extensions.Configuration.Binder 包
        Configuration?.Bind(string.Join(":", sections), list);
        
        return list;
    }


    /// <summary>
    ///     根据路径  configuration["App:Name"];
    /// </summary>
    /// <param name="sectionsPath"></param>
    /// <returns></returns>
    public static string? GetValue(string sectionsPath)
    {
        try
        {
            return Configuration?[sectionsPath];
        }
        catch (Exception)
        {
            // ignored
        }

        return "";
    }
}