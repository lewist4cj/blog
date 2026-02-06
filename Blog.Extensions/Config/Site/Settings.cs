namespace Blog.Extensions.Config;

public class SiteSettings:SettingsValidator
{
    public string? Title { get; set; }
    public string? Logo { get; set; }
    public string? BeiAn { get; set; }
    public bool IsBlogMode { get; set; }

}

public class ProjectSettings:SettingsValidator
{
    public string? Title { get; set; }
    public string? Icon { get; set; }
    public string? Fontend { get; set; }
}

public class SeoSettings:SettingsValidator
{
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
}

public class AboutSettings:SettingsValidator
{
    public string? SiteDate { get; set; }
    public string? SiteVersion { get; set; }
    public string? QqCode { get; set; }
    public string? WechatCode { get; set; }
    public string? Github { get; set; }
    public string? Gitee { get; set; }
    public string? Bilibli { get; set; }

}

public class LoginSettings
{
    public string? Qq { get; set; }
    public string? Email { get; set; }
    public string? UsrPassword { get; set; }
    public string? Captcha { get; set; }
}

public class ComponentSettings:SettingsValidator
{
    public List<ComponentItems> Items { get; set; } = [];
}
public class ComponentItems:SettingsValidator
{
    public string? ComponentName { get; set; }
    public bool Enable { get; set; }
}

public class ArticleSettings:SettingsValidator
{
    public bool Examine { get; set; }
}
