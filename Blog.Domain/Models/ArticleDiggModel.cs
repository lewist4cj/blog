using SqlSugar;

namespace Blog.Domain;

[SugarTable("article_digg_models")]
[SugarIndex("idx_name", nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("idx_name", nameof(ArticleId), OrderByType.Asc, false)]
public class ArticleDiggModel : BaseEntity
{
    public ulong? UserId { get; set; }

    public ulong? ArticleId { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
