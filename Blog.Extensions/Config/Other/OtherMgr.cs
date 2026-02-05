
using Blog.Common;
using Microsoft.Extensions.Configuration;

namespace Blog.Extensions.Config;


public class OtherMgr
{
    public readonly EmailSettings? emailSettings;
    public readonly QiNiuSettings? qiNiuSettings;
    public readonly QqSettings? qqSettings;
    public readonly AiSettings? aiSettings;

    public OtherMgr()
    {
        emailSettings = AppSettings.Configuration!.GetSection("OtherMgr:EmailSettings").Get<EmailSettings>();
        qiNiuSettings = AppSettings.Configuration.GetSection("OtherMgr:QiNiuSettings").Get<QiNiuSettings>();
        qqSettings = AppSettings.Configuration.GetSection("OtherMgr:QqSettings").Get<QqSettings>();
        aiSettings = AppSettings.Configuration.GetSection("OtherMgr:AiSettings").Get<AiSettings>();
     
    }

}