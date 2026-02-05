
namespace Blog.Extensions.Config;

public class EmailSettings
{
    public string? Domain { get; set; }
    public int Port { get; set; }
    public string? SendEmail { get; set; }
    public string? AuthCode { get; set; }
    public string? SendEmailName { get; set; }
    public bool IsTLS { get; set; }
    public bool IsSSL { get; set; }
}

public class QqSettings
{
    public string? AppId { get; set; }
    public string? AppKey { get; set; }
    public string? RedirectUrl { get; set; }

    public string? GetRedirectUrl()
    {
        return $"https://graph.qq.com/oauth2.0/show?which=Login&display=pc&response_type=code&client_id={AppId}&redirect_uri={RedirectUrl}";
    }
}

public class AiSettings
{
    public bool IsEnable { get; set; }
    public string? SecretKey { get; set; }
    public string? NickName { get; set; }
    public string? Avatar { get; set; }
}

public class QiNiuSettings
{
    public bool IsEnable { get; set; }
    public string? AccessKey { get; set; }
    public string? SecretKey { get; set; }
    public string? Bucket { get; set; }
    public string? Domain { get; set; }
    public string? Zone { get; set; }
    public string? Prefix { get; set; }
    public int Size { get; set; }
}