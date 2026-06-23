using SqlSugar;

namespace Blog.Domain;

/// <summary>
/// 站点配置持久化到数据库的实体
/// 将 appsettings.json 中的站点配置保存到数据库，支持运行时动态修改
/// </summary>
[SugarTable("site_configs")]
public class SiteConfigModel : BaseEntity
{
    /// <summary>
    /// 配置节名称，例如 SiteMgr、OtherSiteMgr:EmailSettings
    /// </summary>
    [SugarColumn(Length = 128)]
    public string Section { get; set; } = string.Empty;

    /// <summary>
    /// 配置 JSON 内容
    /// </summary>
    [SugarColumn(Length = -1)]
    public string ConfigValue { get; set; } = string.Empty;

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }
}
