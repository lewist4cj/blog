using SqlSugar;

namespace Blog.Domain;

[SugarTable("article_digg_models")]
[SugarIndex("idx_name", nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("idx_name", nameof(ArticleId), OrderByType.Asc, false)]
public class ArticleDiggModel : BaseEntity
{
    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "article_id")]
    public ulong? ArticleId { get; set; }

    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
