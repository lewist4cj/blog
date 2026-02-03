using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain;

[Keyless]
[Table("user_conf_models")]
[Index("UserId", Name = "uni_user_conf_models_user_id", IsUnique = true)]
public partial class UserConfModel:BaseEntity
{
    [Column("user_id")]
    public ulong? UserId { get;  }

    [Column("like_tags")]
    public string? LikeTags { get; set; }

    [Column("update_username_date", TypeName = "datetime(3)")]
    public DateTime? UpdateUsernameDate { get; set; }

    [Column("publish_collections")]
    public bool? PublishCollections { get; set; }

    [Column("publish_followings")]
    public bool? PublishFollowings { get; set; }

    [Column("publish_fans")]
    public bool? PublishFans { get; set; }

    [Column("theme_style_id")]
    public ulong? ThemeStyleId { get; set; }
}
