using System.ComponentModel.DataAnnotations;

namespace Blog.Common.Database;

/// <summary>
/// 数据库全局配置（读写分离、ES同步、Canal）
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// 默认数据库连接字符串（PostgreSQL）
    /// </summary>
    [Required(ErrorMessage = "DefaultConnection is required")]
    public string DefaultConnection { get; set; } = "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=postgres";

    /// <summary>
    /// 是否启用读写分离
    /// </summary>
    public bool EnableReadWriteSplitting { get; set; } = false;

    /// <summary>
    /// 只读副本连接字符串（读写分离启用时使用）
    /// </summary>
    public string? ReadConnection { get; set; }

    /// <summary>
    /// 是否启用 Elasticsearch 同步
    /// </summary>
    public bool EnableElasticsearchSync { get; set; } = false;

    /// <summary>
    /// Elasticsearch 配置
    /// </summary>
    public ElasticsearchSettings? Elasticsearch { get; set; }

    /// <summary>
    /// 是否启用 Canal CDC 同步
    /// </summary>
    public bool EnableCanalSync { get; set; } = false;

    /// <summary>
    /// Canal 配置
    /// </summary>
    public CanalSettings? Canal { get; set; }
}

/// <summary>
/// Elasticsearch 配置
/// </summary>
public class ElasticsearchSettings
{
    /// <summary>
    /// ES 服务地址，多个用逗号分隔
    /// </summary>
    [Required(ErrorMessage = "Elasticsearch Urls is required when ES sync is enabled")]
    public string Urls { get; set; } = "http://localhost:9200";

    /// <summary>
    /// ES 用户名（可选）
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// ES 密码（可选）
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 索引前缀
    /// </summary>
    public string IndexPrefix { get; set; } = "blog";
}

/// <summary>
/// Canal CDC 配置
/// </summary>
public class CanalSettings
{
    /// <summary>
    /// Canal Server 地址
    /// </summary>
    public string Host { get; set; } = "127.0.0.1";

    /// <summary>
    /// Canal Server 端口
    /// </summary>
    public int Port { get; set; } = 11111;

    /// <summary>
    /// Canal 实例名称
    /// </summary>
    public string Instance { get; set; } = "example";

    /// <summary>
    /// Canal 用户名
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Canal 密码
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 订阅的数据库表，如 "blog\\.article,blog\\.user"
    /// </summary>
    public string? SubscribeFilter { get; set; }
}
