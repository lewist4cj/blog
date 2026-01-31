using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("category_models")]
public class CategoryModel : BaseModel
{
    [MaxLength(32)]
    public string? Title { get; set; }
    
    [Column("user_id")]
    public long? UserId { get; set; }
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
    
    // 反向导航属性（如果需要的话）
}
