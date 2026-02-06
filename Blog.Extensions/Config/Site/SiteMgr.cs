using Blog.Common;
using Microsoft.Extensions.Configuration;

namespace Blog.Extensions.Config;

public class SiteMgr
{
    public SiteSettings? SiteSettings { get; set; }
    public AboutSettings? AboutSettings { get; set; }
    public ArticleSettings? ArticleSettings { get; set; }
    public SeoSettings? SeoSettings { get; set; }
    public ComponentSettings? ComponentSettings { get; set; }
    public LoginSettings? LoginSettings { get; set; }
    public ProjectSettings? ProjectSettings { get; set; }

    public SiteMgr()
    {
        SiteSettings = AppSettings.Configuration!.GetSection("SiteMgr:SiteSettings").Get<SiteSettings>();
        AboutSettings = AppSettings.Configuration.GetSection("SiteMgr:AboutSettings").Get<AboutSettings>();
        ArticleSettings = AppSettings.Configuration.GetSection("SiteMgr:ArticleSettings").Get<ArticleSettings>();
        SeoSettings = AppSettings.Configuration.GetSection("SiteMgr:SeoSettings").Get<SeoSettings>();
        ComponentSettings = AppSettings.Configuration.GetSection("SiteMgr:ComponentSettings").Get<ComponentSettings>();
        LoginSettings = AppSettings.Configuration.GetSection("SiteMgr:LoginSettings").Get<LoginSettings>();
        ProjectSettings = AppSettings.Configuration.GetSection("SiteMgr:ProjectSettings").Get<ProjectSettings>();
    }

    /// <summary>
    /// 获取指定名称的配置节点
    /// </summary>
    /// <param name="nodeName">节点名称</param>
    /// <returns>配置对象</returns>
    public object? GetNodeByName(string nodeName)
    {
        return nodeName?.ToLower() switch
        {
            "site" => SiteSettings,
            "about" => AboutSettings,
            "article" => ArticleSettings,
            "seo" => SeoSettings,
            "component" => ComponentSettings,
            "login" => LoginSettings,
            "project" => ProjectSettings,
            "all" => this, // 根目录，返回整个 SiteMgr 对象
            _ => null
        };
    }

    /// <summary>
    /// 判断指定节点名称是否为根目录
    /// </summary>
    /// <param name="nodeName">节点名称</param>
    /// <returns>如果是根目录则返回true</returns>
    public bool IsRootNode(string? nodeName)
    {
        return string.IsNullOrEmpty(nodeName) || 
               string.Equals(nodeName, "all", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(nodeName, "root", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 获取所有可用的节点名称
    /// </summary>
    /// <returns>节点名称集合</returns>
    public IEnumerable<string> GetAllNodeNames()
    {
        yield return "site";
        yield return "about";
        yield return "article";
        yield return "seo";
        yield return "component";
        yield return "login";
        yield return "project";
        yield return "other";
        yield return "all"; // 表示根目录
    }
}