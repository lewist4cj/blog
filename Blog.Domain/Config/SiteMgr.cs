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
    /// 从 appsettings.json 加载默认配置，AOT 安全（手动绑定，无反射）。
    /// </summary>
    public SiteMgr(IConfiguration configuration)
    {
        var section = configuration.GetSection("SiteMgr");
        SiteSettings = BindSiteSettings(section.GetSection("SiteSettings"));
        AboutSettings = BindAboutSettings(section.GetSection("AboutSettings"));
        ArticleSettings = BindArticleSettings(section.GetSection("ArticleSettings"));
        SeoSettings = BindSeoSettings(section.GetSection("SeoSettings"));
        ComponentSettings = BindComponentSettings(section.GetSection("ComponentSettings"));
        LoginSettings = BindLoginSettings(section.GetSection("LoginSettings"));
        ProjectSettings = BindProjectSettings(section.GetSection("ProjectSettings"));
    }

    private static SiteSettings? BindSiteSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new SiteSettings
        {
            Title = s["Title"],
            Logo = s["Logo"],
            BeiAn = s["BeiAn"],
            IsBlogMode = bool.TryParse(s["IsBlogMode"], out var v) && v
        };
    }

    private static AboutSettings? BindAboutSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new AboutSettings
        {
            SiteDate = s["SiteDate"],
            SiteVersion = s["SiteVersion"],
            QqCode = s["QqCode"],
            WechatCode = s["WechatCode"],
            Github = s["Github"],
            Gitee = s["Gitee"],
            Bilibli = s["Bilibli"]
        };
    }

    private static ArticleSettings? BindArticleSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new ArticleSettings
        {
            Examine = bool.TryParse(s["Examine"], out var v) && v
        };
    }

    private static SeoSettings? BindSeoSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new SeoSettings
        {
            Keywords = s["Keywords"],
            Description = s["Description"],
            Author = s["Author"]
        };
    }

    private static ComponentSettings? BindComponentSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        var items = new List<ComponentItems>();
        var children = s.GetSection("Items").GetChildren();
        foreach (var child in children)
        {
            items.Add(new ComponentItems
            {
                ComponentName = child["ComponentName"],
                Enable = bool.TryParse(child["Enable"], out var v) && v
            });
        }
        return new ComponentSettings { Items = items };
    }

    private static LoginSettings? BindLoginSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new LoginSettings
        {
            Qq = s["Qq"],
            Email = s["Email"],
            UsrPassword = s["UsrPassword"],
            Captcha = s["Captcha"]
        };
    }

    private static ProjectSettings? BindProjectSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new ProjectSettings
        {
            Title = s["Title"],
            Icon = s["Icon"],
            Fontend = s["Fontend"]
        };
    }

    public  object? GetNodeByName(string nodeName)
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

    public static bool IsRootNode(string? nodeName)
    {
        return string.IsNullOrEmpty(nodeName) ||
               string.Equals(nodeName, "all", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(nodeName, "root", StringComparison.OrdinalIgnoreCase);
    }

    public static IEnumerable<string> GetAllNodeNames()
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
