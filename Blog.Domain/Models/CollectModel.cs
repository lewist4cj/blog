using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

[Table("collect_models")]
public partial class CollectModel:BaseEntity
{
    // [Key]
    // [Column("id")]
    // public ulong Id { get; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("title")]
    [StringLength(32)]
    public string? Title { get; set; }

    [Column("abstract")]
    [StringLength(256)]
    public string? Abstract { get; set; }

    [Column("cover")]
    [StringLength(256)]
    public string? Cover { get; set; }

    [Column("article_count")]
    public long? ArticleCount { get; set; }

    [Column("user_id")]
    public ulong? UserId { get; set; }
}
