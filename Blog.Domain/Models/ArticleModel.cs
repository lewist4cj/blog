using SqlSugar;

namespace Blog.Domain;

[SugarTable("article_models")]
public class ArticleModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    public string? Title { get; set; }

    public string? Desc { get; set; }

    public string? Content { get; set; }

    public ulong? ContentId { get; set; }

    public ulong? CategoryId { get; set; }

    public string? TagList { get; set; }

    public string? Cover { get; set; }

    public ulong? UserId { get; set; }

    public long? LookCount { get; set; }

    public long? LikeCount { get; set; }

    public long? CommentCount { get; set; }

    public long? CollectCount { get; set; }

    public bool? EnableComment { get; set; }

    public long? Status { get; set; }

    public bool? UserTop { get; set; }

    public bool? AdminTop { get; set; }
}
