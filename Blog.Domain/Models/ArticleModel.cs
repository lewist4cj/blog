using SqlSugar;

namespace Blog.Domain;

[SugarTable("article_models")]
public class ArticleModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "title")]
    public string? Title { get; set; }

    [SugarColumn(ColumnName = "desc")]
    public string? Desc { get; set; }

    [SugarColumn(ColumnName = "content")]
    public string? Content { get; set; }

    [SugarColumn(ColumnName = "content_id")]
    public ulong? ContentId { get; set; }

    [SugarColumn(ColumnName = "category_id")]
    public ulong? CategoryId { get; set; }

    [SugarColumn(ColumnName = "tag_list")]
    public string? TagList { get; set; }

    [SugarColumn(ColumnName = "cover")]
    public string? Cover { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "look_count")]
    public long? LookCount { get; set; }

    [SugarColumn(ColumnName = "like_count")]
    public long? LikeCount { get; set; }

    [SugarColumn(ColumnName = "comment_count")]
    public long? CommentCount { get; set; }

    [SugarColumn(ColumnName = "collect_count")]
    public long? CollectCount { get; set; }

    [SugarColumn(ColumnName = "enable_comment")]
    public bool? EnableComment { get; set; }

    [SugarColumn(ColumnName = "status")]
    public long? Status { get; set; }

    [SugarColumn(ColumnName = "user_top")]
    public bool? UserTop { get; set; }

    [SugarColumn(ColumnName = "admin_top")]
    public bool? AdminTop { get; set; }
}
