using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Config;

public abstract class SettingsValidator : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        return results;
    }
}

public class SiteSettings : SettingsValidator
{
    public string? Title { get; set; }
    public string? Logo { get; set; }
    public string? BeiAn { get; set; }
    public bool IsBlogMode { get; set; }
}

public class ProjectSettings : SettingsValidator
{
    public string? Title { get; set; }
    public string? Icon { get; set; }
    public string? Fontend { get; set; }
}

public class SeoSettings : SettingsValidator
{
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
}

public class AboutSettings : SettingsValidator
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

public class ComponentSettings : SettingsValidator
{
    public List<ComponentItems> Items { get; set; } = [];
}

public class ComponentItems : SettingsValidator
{
    public string? ComponentName { get; set; }
    public bool Enable { get; set; }
}

public class ArticleSettings : SettingsValidator
{
    public bool Examine { get; set; }
}

public class EmailSettings : SettingsValidator
{
    [Required(ErrorMessage = "Domain is required.")]
    public string? Domain { get; set; }
    [Required(ErrorMessage = "Port is required.")]
    public int Port { get; set; }
    [Required(ErrorMessage = "SendEmail is required.")]
    public string? SendEmail { get; set; }
    [Required(ErrorMessage = "AuthCode is required.")]
    public string? AuthCode { get; set; }
    [Required(ErrorMessage = "SendEmailName is required.")]
    public string? SendEmailName { get; set; }
    [Required(ErrorMessage = "IsTLS is required.")]
    public bool IsTLS { get; set; }
    [Required(ErrorMessage = "IsSSL is required.")]
    public bool IsSSL { get; set; }
}

public class QqSettings : SettingsValidator
{
    [Required(ErrorMessage = "AppId is required.")]
    public string? AppId { get; set; }
    [Required(ErrorMessage = "AppKey is required.")]
    public string? AppKey { get; set; }
    [Required(ErrorMessage = "RedirectUrl is required.")]
    public string? RedirectUrl { get; set; }

    public string? GetRedirectUrl()
    {
        return $"https://graph.qq.com/oauth2.0/show?which=Login&display=pc&response_type=code&client_id={AppId}&redirect_uri={RedirectUrl}";
    }
}

public class AiSettings : SettingsValidator
{
    public bool IsEnable { get; set; }
    [Required(ErrorMessage = "SecretKey is required.")]
    public string? SecretKey { get; set; }
    [Required(ErrorMessage = "NickName is required.")]
    public string? NickName { get; set; }
    public string? Avatar { get; set; }
}

public class QiNiuSettings : SettingsValidator
{
    public bool IsEnable { get; set; }
    [Required(ErrorMessage = "AccessKey is required.")]
    public string? AccessKey { get; set; }
    [Required(ErrorMessage = "SecretKey is required.")]
    public string? SecretKey { get; set; }
    [Required(ErrorMessage = "Bucket is required.")]
    public string? Bucket { get; set; }
    [Required(ErrorMessage = "Domain is required.")]
    public string? Domain { get; set; }
    [Required(ErrorMessage = "Zone is required.")]
    public string? Zone { get; set; }
    public string? Prefix { get; set; }
    public int Size { get; set; } = 1024;
}
