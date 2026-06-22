using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_article_look_history_models")]
public class UserArticleLookHistoryModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "article_id")]
    public ulong? ArticleId { get; set; }
}
