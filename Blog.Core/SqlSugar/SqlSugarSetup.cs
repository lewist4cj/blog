using Blog.Common.Database;
using Blog.Domain;
using SqlSugar;

namespace Blog.Core.SqlSugar;

/// <summary>
/// SqlSugar 数据库配置
/// </summary>
public static class SqlSugarSetup
{
    private static bool _aotConfigured;

    /// <summary>
    /// 初始化 SqlSugar 全局设置（AOT 模式等）
    /// 在注册 ISqlSugarClient 前调用一次
    /// </summary>
    public static void ConfigureGlobal()
    {
        if (_aotConfigured) return;
        _aotConfigured = true;

        // 启用 AOT 模式（禁用运行时动态类型生成）
        StaticConfig.EnableAot = true;
    }

    /// <summary>
    /// 从 DatabaseSettings 创建 SqlSugarClient，支持读写分离
    /// </summary>
    public static ISqlSugarClient CreateClient(DatabaseSettings settings)
    {
        ConfigureGlobal();

        var config = new ConnectionConfig
        {
            DbType = DbType.PostgreSQL,
            ConnectionString = settings.DefaultConnection,
            IsAutoCloseConnection = true,
            MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true
            }
        };

        // 读写分离：配置从库连接
        if (settings.EnableReadWriteSplitting && !string.IsNullOrEmpty(settings.ReadConnection))
        {
            config.SlaveConnectionConfigs =
            [
                new() { HitRate = 100, ConnectionString = settings.ReadConnection }
            ];
        }

        return new SqlSugarClient(config);
    }

    /// <summary>
    /// CodeFirst 自动同步数据库表结构
    /// 在应用启动时调用，确保所有实体对应的表存在
    /// </summary>
    public static void InitDatabase(ISqlSugarClient db)
    {
        db.CodeFirst.InitTables(
            typeof(ArticleModel),
            typeof(ArticleDiggModel),
            typeof(BannerModel),
            typeof(CategoryModel),
            typeof(CollectModel),
            typeof(CommentModel),
            typeof(GlobalNotication),
            typeof(LogModel),
            typeof(SiteConfigModel),
            typeof(UserArticleCollectModel),
            typeof(UserArticleLookHistoryModel),
            typeof(UserConfModel),
            typeof(UserModel)
        );
    }
}
