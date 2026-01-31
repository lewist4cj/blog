using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("comment_models")]
public class CommentModel : BaseModel
{
    [MaxLength(256)]
    public string? Content { get; set; }
    
    [Column("user_id")]
    public long? UserId { get; set; }
    
    [Column("article_id")]
    public long? ArticleId { get; set; }
    
    [Column("parent_id")]
    public long? ParentId { get; set; }
    
    [Column("root_parent_id")]
    public long? RootParentId { get; set; }
    
    [Column("digg_count")]
    public long? DiggCount { get; set; }
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
    
    [ForeignKey(nameof(ArticleId))]
    public ArticleModel? ArticleModel { get; set; }
    
    [ForeignKey(nameof(ParentId))]
    public CommentModel? ParentComment { get; set; }
    
    [ForeignKey(nameof(RootParentId))]
    public CommentModel? RootParentComment { get; set; }
}