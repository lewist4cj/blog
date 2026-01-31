using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("user_article_collect_models")]
public class UserArticleCollectModel
{
    [Column("user_id")]
    public long? UserId { get; set; }
    
    [Column("article_id")]
    public long? ArticleId { get; set; }  // 改为 long? 与 BaseModel.Id 类型匹配
    
    [Column("collect_id")]
    public long? CollectId { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
    
    [ForeignKey(nameof(ArticleId))]
    public ArticleModel? ArticleModel { get; set; }
    
    [ForeignKey(nameof(CollectId))]
    public CollectModel? CollectModel { get; set; }
}