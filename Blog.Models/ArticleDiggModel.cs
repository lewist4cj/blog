using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models;

[Keyless]
[Table("article_digg_models")]
[Index("UserId", "ArticleId", Name = "idx_name", IsUnique = true)]
public partial class ArticleDiggModel
{
    [Column("user_id")]
    public ulong? UserId { get; set; }

    [Column("article_id")]
    public ulong? ArticleId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
