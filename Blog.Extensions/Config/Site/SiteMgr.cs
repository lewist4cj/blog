using Blog.Common;
using Microsoft.Extensions.Configuration;

namespace Blog.Extensions.Config;


public class SiteMgr
{
    public readonly SiteSettings? siteSettings;
    public readonly AboutSettings? aboutSettings;
    public readonly ArticleSettings? articleSettings;
    public readonly SeoSettings? seoSettings;
    public readonly ComponentSettings? componentSettings;
    public readonly LoginSettings? loginSettings;
    public readonly ProjectSettings? projectSettings;

    public SiteMgr()
    {
        siteSettings = AppSettings.Configuration!.GetSection("SiteMgr:SiteSettings").Get<SiteSettings>();
        aboutSettings = AppSettings.Configuration.GetSection("SiteMgr:AboutSettings").Get<AboutSettings>();
        articleSettings = AppSettings.Configuration.GetSection("SiteMgr:ArticleSettings").Get<ArticleSettings>();
        seoSettings = AppSettings.Configuration.GetSection("SiteMgr:SeoSettings").Get<SeoSettings>();
        componentSettings = AppSettings.Configuration.GetSection("SiteMgr:ComponentSettings").Get<ComponentSettings>();
        loginSettings = AppSettings.Configuration.GetSection("SiteMgr:LoginSettings").Get<LoginSettings>();
        projectSettings = AppSettings.Configuration.GetSection("SiteMgr:ProjectSettings").Get<ProjectSettings>();
    }

}