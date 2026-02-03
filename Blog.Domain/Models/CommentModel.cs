using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

[Table("comment_models")]
public partial class CommentModel:BaseEntity
{
    // [Key]
    // [Column("id")]
    // public ulong Id { get; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("content")]
    [StringLength(256)]
    public string? Content { get; set; }

    [Column("user_id")]
    public ulong? UserId { get; set; }

    [Column("article_id")]
    public ulong? ArticleId { get; set; }

    [Column("parent_id")]
    public ulong? ParentId { get; set; }

    [Column("root_parent_id")]
    public ulong? RootParentId { get; set; }

    [Column("digg_count")]
    public long? DiggCount { get; set; }
}
