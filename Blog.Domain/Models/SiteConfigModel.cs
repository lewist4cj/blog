using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

/// <summary>
/// 站点配置持久化到数据库的实体
/// 将 appsettings.json 中的站点配置保存到数据库，支持运行时动态修改
/// </summary>
[Table("site_configs")]
public partial class SiteConfigModel : BaseEntity
{
    /// <summary>
    /// 配置节名称，例如 SiteMgr、OtherSiteMgr:EmailSettings
    /// </summary>
    [Column("section")]
    [StringLength(128)]
    public string Section { get; set; } = string.Empty;

    /// <summary>
    /// 配置 JSON 内容
    /// </summary>
    [Column("config_value")]
    public string ConfigValue { get; set; } = string.Empty;

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }
}
