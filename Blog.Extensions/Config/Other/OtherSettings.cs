
using System.ComponentModel.DataAnnotations;

namespace Blog.Extensions.Config;

public abstract class SettingsValidator : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        return results;
    }
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