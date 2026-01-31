using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("article_models")]
public class ArticleModel : BaseModel
{
    [Column(TypeName = "longtext")]
    public string? Title { get; set; }
    
    [Column(TypeName = "longtext")]
    public string? Desc { get; set; }
    
    [Column(TypeName = "longtext")]
    public string? Content { get; set; }
    
    [Column("content_id")]
    public ulong? ContentId { get; set; }
    
    [Column("tag_list", TypeName = "longtext")]
    public string? TagList { get; set; }
    
    [Column(TypeName = "longtext")]
    public string? Cover { get; set; }
    
    [Column("user_id")]
    public long? UserId { get; set; }
    
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
    
    public long? Status { get; set; }
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
    
    // 反向导航属性
    public ICollection<CommentModel>? Comments { get; set; }
    
    public ICollection<ArticleDiggModel>? ArticleDiggs { get; set; }
    
    public ICollection<UserArticleCollectModel>? UserArticleCollects { get; set; }
    
    public ICollection<UserArticleLookHistoryModel>? UserArticleLookHistories { get; set; }
}