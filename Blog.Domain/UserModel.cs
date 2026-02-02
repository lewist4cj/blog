using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain;

[Table("user_models")]
public partial class UserModel:BaseEntity
{
    // [Key]
    // [Column("id")]
    // public ulong Id { get;}

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("username")]
    [StringLength(32)]
    public string? Username { get; set; }

    [Column("nickname")]
    [StringLength(32)]
    public string? Nickname { get; set; }

    [Column("password")]
    [StringLength(64)]
    public string? Password { get; set; }

    [Column("avatar")]
    [StringLength(256)]
    public string? Avatar { get; set; }

    [Column("abstract")]
    [StringLength(256)]
    public string? Abstract { get; set; }

    [Column("register_src")]
    public sbyte? RegisterSrc { get; set; }

    [Column("code_age")]
    public long? CodeAge { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("open_id")]
    [StringLength(126)]
    public string? OpenId { get; set; }

    [Column("role")]
    public sbyte? Role { get; set; }
}
