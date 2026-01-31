using Blog.Common;
using blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Core.DbContext;

// 添加 snake_case 转换扩展方法
public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        result.Append(char.ToLower(input[0]));

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append('_');
                result.Append(char.ToLower(input[i]));
            }
            else
            {
                result.Append(input[i]);
            }
        }

        return result.ToString();
    }
}

public class BlogDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<ArticleDiggModel> ArticleDiggs { get; set; }
    
    public DbSet<ArticleModel> Articles { get; set; }
    
    public DbSet<BannerModel> Banners { get; set; }
    
    public DbSet<CategoryModel> Categories { get; set; }
    
    public DbSet<CollectModel> Collects { get; set; }
    
    public DbSet<CommentModel> Comments { get; set; }
    
    public DbSet<GlobalNotificationModel> GlobalNotifications { get; set; }
    
    public DbSet<LogModel> Logs { get; set; }
    
    public DbSet<UserArticleCollectModel> UserArticleCollects { get; set; }
    
    public DbSet<UserArticleLookHistoryModel> UserArticleLookHistories { get; set; }
    
    public DbSet<UserConfModel> UserConfigs { get; set; }
    
    public DbSet<UserModel> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (AppSettings.Configuration != null)
            {
                var connectionString = AppSettings.Configuration.GetConnectionString("DefaultConnection");
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 全局配置 snake_case 命名约定
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // 表名使用 snake_case
            entityType.SetTableName(entityType.GetTableName()?.ToSnakeCase());
            
            // 属性名使用 snake_case
            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }
        }
        
        // 明确忽略 BaseModel，防止 EF Core 为其创建表
        modelBuilder.Ignore<BaseModel>();
        
        // 配置 ArticleDiggModel 的复合主键
        modelBuilder.Entity<ArticleDiggModel>()
            .HasKey(x => new { x.UserId, x.ArticleId });
            
        // 配置 UserArticleCollectModel 的复合主键
        modelBuilder.Entity<UserArticleCollectModel>()
            .HasKey(x => new { x.UserId, x.ArticleId, x.CollectId });
            
        // 配置复合唯一索引
        modelBuilder.Entity<ArticleDiggModel>()
            .HasIndex(x => new { x.UserId, x.ArticleId })
            .IsUnique()
            .HasDatabaseName("idx_name");
            
        modelBuilder.Entity<UserArticleCollectModel>()
            .HasIndex(x => new { x.UserId, x.ArticleId, x.CollectId })
            .IsUnique()
            .HasDatabaseName("idx_name");
            
        // 配置 UserConfModel 的唯一约束
        modelBuilder.Entity<UserConfModel>()
            .HasIndex(x => x.UserId)
            .IsUnique()
            .HasDatabaseName("uni_user_conf_models_user_id");
            
        // 配置列名映射以匹配原始SQL
        modelBuilder.Entity<ArticleDiggModel>()
            .Property(x => x.CreatedAt)
            .HasColumnName("created_at");
            
        modelBuilder.Entity<UserArticleCollectModel>()
            .Property(x => x.CreatedAt)
            .HasColumnName("created_at");
            
        // 配置时间戳行为 - 使用正确的 CURRENT_TIMESTAMP(6) 语法
        modelBuilder.Entity<ArticleModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<ArticleModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<CategoryModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<CategoryModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<CollectModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<CollectModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<CommentModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<CommentModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<GlobalNotificationModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<GlobalNotificationModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<LogModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<LogModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<UserArticleLookHistoryModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<UserArticleLookHistoryModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<UserModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<UserModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        modelBuilder.Entity<UserConfModel>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        modelBuilder.Entity<UserConfModel>()
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAddOrUpdate();
            
        // 配置布尔值存储 - 使用 tinyint(1) 而不是 tinyint unsigned
        modelBuilder.Entity<ArticleModel>()
            .Property(x => x.EnableComment)
            .HasConversion<int>(); // tinyint(1)
            
        modelBuilder.Entity<LogModel>()
            .Property(x => x.IsRead)
            .HasConversion<int>(); // tinyint(1)
            
        modelBuilder.Entity<LogModel>()
            .Property(x => x.LoginStatus)
            .HasConversion<int>(); // tinyint(1)
            
        modelBuilder.Entity<UserConfModel>()
            .Property(x => x.PublishCollections)
            .HasConversion<int>(); // tinyint(1)
            
        modelBuilder.Entity<UserConfModel>()
            .Property(x => x.PublishFollowings)
            .HasConversion<int>(); // tinyint(1)
            
        modelBuilder.Entity<UserConfModel>()
            .Property(x => x.PublishFans)
            .HasConversion<int>(); // tinyint(1)
            
        // 移除外键配置，匹配原始SQL结构（无外键约束）
    }
}