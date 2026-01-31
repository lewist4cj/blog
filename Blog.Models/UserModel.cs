using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("user_models")]
public class UserModel : BaseModel
{
    [MaxLength(32)]
    public string? Username { get; set; }
    
    [MaxLength(32)]
    public string? Nickname { get; set; }
    
    [MaxLength(64)]
    public string? Password { get; set; }
    
    [MaxLength(256)]
    public string? Avatar { get; set; }
    
    [MaxLength(256)]
    public string? Abstract { get; set; }
    
    [Column("register_src")]
    public sbyte? RegisterSrc { get; set; }
    
    [Column("code_age")]
    public long? CodeAge { get; set; }
    
    [MaxLength(256)]
    public string? Email { get; set; }
    
    [Column("open_id")]
    [MaxLength(126)]
    public string? OpenId { get; set; }
    
    [Column("role")]
    public sbyte? Role { get; set; } // 1-admin 2-guest 3-visitor
    
    // 反向导航属性
    public ICollection<ArticleModel>? Articles { get; set; }
    
    public ICollection<CategoryModel>? Categories { get; set; }
    
    public ICollection<CollectModel>? Collects { get; set; }
    
    public ICollection<CommentModel>? Comments { get; set; }
    
    public ICollection<LogModel>? Logs { get; set; }
    
    public UserConfModel? UserConfig { get; set; }
    
    public ICollection<ArticleDiggModel>? ArticleDiggs { get; set; }
    
    public ICollection<UserArticleCollectModel>? UserArticleCollects { get; set; }
    
    public ICollection<UserArticleLookHistoryModel>? UserArticleLookHistories { get; set; }
}