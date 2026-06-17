using Microsoft.Extensions.Configuration;

namespace Blog.Domain.Config;

public class OtherSiteMgr
{
    public EmailSettings? EmailSettings { get; set; }
    public QiNiuSettings? QiNiuSettings { get; set; }
    public QqSettings? QqSettings { get; set; }
    public AiSettings? AiSettings { get; set; }

    public OtherSiteMgr() { }

    public OtherSiteMgr(IConfiguration configuration)
    {
        var section = configuration.GetSection("OtherSiteMgr");
        EmailSettings = section.GetSection("EmailSettings").Get<EmailSettings>();
        QiNiuSettings = section.GetSection("QiNiuSettings").Get<QiNiuSettings>();
        QqSettings = section.GetSection("QqSettings").Get<QqSettings>();
        AiSettings = section.GetSection("AiSettings").Get<AiSettings>();
    }
}
