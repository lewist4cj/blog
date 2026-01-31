using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace blog.Models;

[Table("user_conf_models")]
public class UserConfModel : BaseModel
{
    [Column("user_id")]
    public long UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public UserModel UserModel { get; set; } = null!;
    
    [Column(TypeName = "longtext")]
    public List<string> LikeTags { get; set; } = new();
    
    [Column("update_username_date")]
    public DateTime? UpdateUsernameDate { get; set; }
    
    [Column("publish_collections")]
    public bool PublishCollections { get; set; }
    
    [Column("publish_followings")]
    public bool PublishFollowings { get; set; }
    
    [Column("publish_fans")]
    public bool PublishFans { get; set; }
    
    [Column("theme_style_id")]
    public long? ThemeStyleId { get; set; }
}