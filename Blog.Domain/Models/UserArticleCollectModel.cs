using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain;

[Table("user_article_collect_models")]
[Index("UserId", "ArticleId", "CollectId", Name = "idx_name", IsUnique = true)]
public partial class UserArticleCollectModel:BaseEntity
{
    [Column("user_id")]
    public ulong? UserId { get; set; }

    [Column("article_id")]
    public ulong? ArticleId { get; set; }

    [Column("collect_id")]
    public ulong? CollectId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
