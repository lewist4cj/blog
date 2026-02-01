using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("user_article_look_history_models")]
public partial class UserArticleLookHistoryModel
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("user_id")]
    public ulong? UserId { get; set; }

    [Column("article_id")]
    public ulong? ArticleId { get; set; }
}
