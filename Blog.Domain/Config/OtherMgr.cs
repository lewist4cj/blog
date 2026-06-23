using Microsoft.Extensions.Configuration;

namespace Blog.Domain.Config;

public class OtherSiteMgr
{
    public EmailSettings? EmailSettings { get; set; }
    public QiNiuSettings? QiNiuSettings { get; set; }
    public QqSettings? QqSettings { get; set; }
    public AiSettings? AiSettings { get; set; }

    public OtherSiteMgr() { }

    /// <summary>
    /// 从 appsettings.json 加载默认配置，AOT 安全（手动绑定，无反射）。
    /// </summary>
    public OtherSiteMgr(IConfiguration configuration)
    {
        var section = configuration.GetSection("OtherSiteMgr");
        EmailSettings = BindEmailSettings(section.GetSection("EmailSettings"));
        QiNiuSettings = BindQiNiuSettings(section.GetSection("QiNiuSettings"));
        QqSettings = BindQqSettings(section.GetSection("QqSettings"));
        AiSettings = BindAiSettings(section.GetSection("AiSettings"));
    }

    private static EmailSettings? BindEmailSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new EmailSettings
        {
            Domain = s["Domain"],
            Port = int.TryParse(s["Port"], out var p) ? p : 0,
            SendEmail = s["SendEmail"],
            AuthCode = s["AuthCode"],
            SendEmailName = s["SendEmailName"],
            IsTLS = bool.TryParse(s["IsTLS"], out var tls) && tls,
            IsSSL = bool.TryParse(s["IsSSL"], out var ssl) && ssl
        };
    }

    private static QiNiuSettings? BindQiNiuSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new QiNiuSettings
        {
            IsEnable = bool.TryParse(s["IsEnable"], out var en) && en,
            AccessKey = s["AccessKey"],
            SecretKey = s["SecretKey"],
            Bucket = s["Bucket"],
            Domain = s["Domain"],
            Zone = s["Zone"],
            Prefix = s["Prefix"],
            Size = int.TryParse(s["Size"], out var sz) ? sz : 1024
        };
    }

    private static QqSettings? BindQqSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new QqSettings
        {
            AppId = s["AppId"],
            AppKey = s["AppKey"],
            RedirectUrl = s["RedirectUrl"]
        };
    }

    private static AiSettings? BindAiSettings(IConfigurationSection s)
    {
        if (!s.Exists()) return null;
        return new AiSettings
        {
            IsEnable = bool.TryParse(s["IsEnable"], out var en) && en,
            SecretKey = s["SecretKey"],
            NickName = s["NickName"],
            Avatar = s["Avatar"]
        };
    }
}
