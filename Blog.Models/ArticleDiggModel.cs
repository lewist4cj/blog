using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("article_digg_models")]
public class ArticleDiggModel
{
    [Column("user_id")]
    public long? UserId { get; set; }
    
    [Column("article_id")]
    public long? ArticleId { get; set; }  // 改为 long? 与 BaseModel.Id 类型匹配
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
    
    [ForeignKey(nameof(ArticleId))]
    public ArticleModel? ArticleModel { get; set; }
}