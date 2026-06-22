using SqlSugar;

namespace Blog.Domain;

[SugarTable("comment_models")]
public class CommentModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "content", Length = 256)]
    public string? Content { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "article_id")]
    public ulong? ArticleId { get; set; }

    [SugarColumn(ColumnName = "parent_id")]
    public ulong? ParentId { get; set; }

    [SugarColumn(ColumnName = "root_parent_id")]
    public ulong? RootParentId { get; set; }

    [SugarColumn(ColumnName = "digg_count")]
    public long? DiggCount { get; set; }
}
