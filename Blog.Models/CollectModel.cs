using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("collect_models")]
public class CollectModel : BaseModel
{
    [MaxLength(32)]
    public string? Title { get; set; }
    
    [MaxLength(256)]
    public string? Abstract { get; set; }
    
    [MaxLength(256)]
    public string? Cover { get; set; }
    
    [Column("article_count")]
    public long? ArticleCount { get; set; }
    
    [Column("user_id")]
    public long? UserId { get; set; }
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
}