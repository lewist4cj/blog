using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_article_collect_models")]
[SugarIndex("idx_name", nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("idx_name", nameof(ArticleId), OrderByType.Asc, false)]
[SugarIndex("idx_name", nameof(CollectId), OrderByType.Asc, false)]
public class UserArticleCollectModel : BaseEntity
{
    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "article_id")]
    public ulong? ArticleId { get; set; }

    [SugarColumn(ColumnName = "collect_id")]
    public ulong? CollectId { get; set; }

    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
