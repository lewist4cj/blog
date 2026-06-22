using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_article_collect_models")]
[SugarIndex("idx_name", nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("idx_name", nameof(ArticleId), OrderByType.Asc, false)]
[SugarIndex("idx_name", nameof(CollectId), OrderByType.Asc, false)]
public class UserArticleCollectModel : BaseEntity
{
    public ulong? UserId { get; set; }

    public ulong? ArticleId { get; set; }

    public ulong? CollectId { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
