
using Blog.Common;
using Microsoft.Extensions.Configuration;

namespace Blog.Extensions.Config;


public class OtherSiteMgr
{
    public readonly EmailSettings? emailSettings;
    public readonly QiNiuSettings? qiNiuSettings;
    public readonly QqSettings? qqSettings;
    public readonly AiSettings? aiSettings;

    public OtherSiteMgr()
    {
        emailSettings = AppSettings.Configuration!.GetSection("OtherSiteMgr:EmailSettings").Get<EmailSettings>();
        qiNiuSettings = AppSettings.Configuration.GetSection("OtherSiteMgr:QiNiuSettings").Get<QiNiuSettings>();
        qqSettings = AppSettings.Configuration.GetSection("OtherSiteMgr:QqSettings").Get<QqSettings>();
        aiSettings = AppSettings.Configuration.GetSection("OtherSiteMgr:AiSettings").Get<AiSettings>();
    }

}