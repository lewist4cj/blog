using Blog.Common;
using Blog.Domain.Config;

namespace Blog.Services.ConfigMgrApp;

/// <summary>
/// 站点配置服务——支持从 DB 读取/写入配置，回退到 appsettings.json
/// </summary>
public interface ISiteConfigService : ITag
{
    Task<SiteMgr> GetSiteMgrAsync();
    Task<OtherSiteMgr> GetOtherSiteMgrAsync();
    Task SaveSiteMgrAsync(SiteMgr siteMgr);
    Task SaveOtherSectionAsync(string sectionName, object settings);
}
