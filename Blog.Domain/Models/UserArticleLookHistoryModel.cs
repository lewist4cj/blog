using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_article_look_history_models")]
public class UserArticleLookHistoryModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    public ulong? UserId { get; set; }

    public ulong? ArticleId { get; set; }
}
