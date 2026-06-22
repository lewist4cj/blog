using System.Diagnostics.CodeAnalysis;
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
    /// 从 appsettings.json 加载默认配置。
    /// 这些类型通过对应的属性访问保留，AOT 下可用。
    /// </summary>
    [RequiresUnreferencedCode("OtherSiteMgr subtypes are preserved by direct property access")]
    [RequiresDynamicCode("OtherSiteMgr subtypes are preserved by direct property access")]
    public OtherSiteMgr(IConfiguration configuration)
    {
        var section = configuration.GetSection("OtherSiteMgr");
#pragma warning disable IL3050, IL2026
        EmailSettings = section.GetSection("EmailSettings").Get<EmailSettings>();
        QiNiuSettings = section.GetSection("QiNiuSettings").Get<QiNiuSettings>();
        QqSettings = section.GetSection("QqSettings").Get<QqSettings>();
        AiSettings = section.GetSection("AiSettings").Get<AiSettings>();
#pragma warning restore IL3050, IL2026
    }
}
