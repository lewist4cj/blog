using Microsoft.EntityFrameworkCore;
using Blog.Domain;
using Microsoft.Extensions.Logging;

namespace Blog.Core.DbContext;

public partial class BlogDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options, ILogger<BlogDbContext> logger)
        : base(options)
    {
    }

    public virtual DbSet<ArticleDiggModel> ArticleDiggModels { get; set; }
    public virtual DbSet<ArticleModel> ArticleModels { get; set; }
    public virtual DbSet<BannerModel> BannerModels { get; set; }
    public virtual DbSet<CategoryModel> CategoryModels { get; set; }
    public virtual DbSet<CollectModel> CollectModels { get; set; }
    public virtual DbSet<CommentModel> CommentModels { get; set; }
    public virtual DbSet<GlobalNotication> GlobalNotications { get; set; }
    public virtual DbSet<LogModel> LogModels { get; set; }
    public virtual DbSet<UserArticleCollectModel> UserArticleCollectModels { get; set; }
    public virtual DbSet<UserArticleLookHistoryModel> UserArticleLookHistoryModels { get; set; }
    public virtual DbSet<UserConfModel> UserConfModels { get; set; }
    public virtual DbSet<UserModel> UserModels { get; set; }
    public virtual DbSet<SiteConfigModel> SiteConfigModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 配置通用默认值
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdAt = entityType.FindProperty("CreatedAt");
            if (createdAt != null && createdAt.ClrType == typeof(DateTime))
            {
                createdAt.SetDefaultValueSql("CURRENT_TIMESTAMP");
            }
        }

        modelBuilder.Entity<ArticleDiggModel>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ArticleModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<BannerModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<CategoryModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<CollectModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<CommentModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<GlobalNotication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<LogModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserArticleCollectModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserArticleLookHistoryModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
