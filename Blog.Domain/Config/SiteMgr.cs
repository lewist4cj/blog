using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Blog.Domain.Config;

public class SiteMgr
{
    public SiteSettings? SiteSettings { get; set; }
    public AboutSettings? AboutSettings { get; set; }
    public ArticleSettings? ArticleSettings { get; set; }
    public SeoSettings? SeoSettings { get; set; }
    public ComponentSettings? ComponentSettings { get; set; }
    public LoginSettings? LoginSettings { get; set; }
    public ProjectSettings? ProjectSettings { get; set; }

    public SiteMgr() { }

    /// <summary>
    /// 从 appsettings.json 加载默认配置。
    /// 这些类型通过对应的属性访问保留，AOT 下可用。
    /// </summary>
    [RequiresUnreferencedCode("SiteMgr subtypes are preserved by direct property access")]
    [RequiresDynamicCode("SiteMgr subtypes are preserved by direct property access")]
    public SiteMgr(IConfiguration configuration)
    {
        var section = configuration.GetSection("SiteMgr");
#pragma warning disable IL3050, IL2026
        SiteSettings = section.GetSection("SiteSettings").Get<SiteSettings>();
        AboutSettings = section.GetSection("AboutSettings").Get<AboutSettings>();
        ArticleSettings = section.GetSection("ArticleSettings").Get<ArticleSettings>();
        SeoSettings = section.GetSection("SeoSettings").Get<SeoSettings>();
        ComponentSettings = section.GetSection("ComponentSettings").Get<ComponentSettings>();
        LoginSettings = section.GetSection("LoginSettings").Get<LoginSettings>();
        ProjectSettings = section.GetSection("ProjectSettings").Get<ProjectSettings>();
#pragma warning restore IL3050, IL2026
    }

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
            "all" => this,
            _ => null
        };
    }

    public bool IsRootNode(string? nodeName)
    {
        return string.IsNullOrEmpty(nodeName) ||
               string.Equals(nodeName, "all", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(nodeName, "root", StringComparison.OrdinalIgnoreCase);
    }

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
        yield return "all";
    }
}
