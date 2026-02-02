using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

[Table("article_models")]
public partial class ArticleModel:BaseEntity
{
    // [Key]
    // [Column("id")]
    // public ulong Id { get;}

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("title")]
    public string? Title { get; set; }

    [Column("desc")]
    public string? Desc { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("content_id")]
    public ulong? ContentId { get; set; }

    [Column("tag_list")]
    public string? TagList { get; set; }

    [Column("cover")]
    public string? Cover { get; set; }

    [Column("user_id")]
    public ulong? UserId { get; set; }

    [Column("look_count")]
    public long? LookCount { get; set; }

    [Column("like_count")]
    public long? LikeCount { get; set; }

    [Column("comment_count")]
    public long? CommentCount { get; set; }

    [Column("collect_count")]
    public long? CollectCount { get; set; }

    [Column("enable_comment")]
    public bool? EnableComment { get; set; }

    [Column("status")]
    public long? Status { get; set; }
}
