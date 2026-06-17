using Blog.Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Core.DbContext;

/// <summary>
/// DbContext 工厂 - 支持读写分离路由
/// 当读写分离禁用时，读写都使用同一个连接
/// </summary>
public class DbContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public DbContextFactory(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// 获取数据库设置
    /// </summary>
    public DatabaseSettings GetSettings()
    {
        var settings = new DatabaseSettings();
        _configuration.GetSection("Database").Bind(settings);

        // 回退到 ConnectionStrings:DefaultConnection
        if (string.IsNullOrEmpty(settings.DefaultConnection))
        {
            settings.DefaultConnection = _configuration.GetConnectionString("DefaultConnection")
                ?? "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=postgres";
        }

        return settings;
    }

    /// <summary>
    /// 创建写入 DbContext（主库）
    /// </summary>
    public BlogDbContext CreateWriteContext()
    {
        var settings = GetSettings();
        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseNpgsql(settings.DefaultConnection, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("Blog.Core");
        });
        optionsBuilder.UseLoggerFactory(_loggerFactory);

        return new BlogDbContext(optionsBuilder.Options,
            _loggerFactory.CreateLogger<BlogDbContext>());
    }

    /// <summary>
    /// 创建只读 DbContext（从库/副本）
    /// 如果读写分离未启用，则使用主库连接
    /// </summary>
    public BlogDbContext CreateReadContext()
    {
        var settings = GetSettings();
        var connectionString = settings.EnableReadWriteSplitting && !string.IsNullOrEmpty(settings.ReadConnection)
            ? settings.ReadConnection
            : settings.DefaultConnection;

        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("Blog.Core");
        });
        optionsBuilder.UseLoggerFactory(_loggerFactory);

        return new BlogDbContext(optionsBuilder.Options,
            _loggerFactory.CreateLogger<BlogDbContext>());
    }

    /// <summary>
    /// 配置 DbContext 选项（用于 DI 注册）
    /// </summary>
    public static void ConfigureWriteContext(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection")
                      ?? configuration.GetSection("Database:DefaultConnection").Value
                      ?? "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connStr, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("Blog.Core");
        });
    }
}
