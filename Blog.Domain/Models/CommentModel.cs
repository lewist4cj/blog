using SqlSugar;

namespace Blog.Domain;

[SugarTable("comment_models")]
public class CommentModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(Length = 256)]
    public string? Content { get; set; }

    public ulong? UserId { get; set; }

    public ulong? ArticleId { get; set; }

    public ulong? ParentId { get; set; }

    public ulong? RootParentId { get; set; }

    public long? DiggCount { get; set; }
}
