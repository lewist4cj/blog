using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using blog.Models.enums.Log;

namespace Blog.Domain;

[Table("log_models")]
public partial class LogModel:BaseEntity
{
    // [Key]
    // [Column("id")]
    // public ulong Id { get; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [Column("log_type")]
    public LogTypeEnum? LogType { get; set; }

    [Column("title")]
    [StringLength(32)]
    public string? Title { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("level")]
    public LogLevelEnum? Level { get; set; }

    [Column("user_id")]
    public ulong? UserId { get; set; }

    [Column("ip")]
    [StringLength(32)]
    public string? Ip { get; set; }

    [Column("addr")]
    [StringLength(64)]
    public string? Addr { get; set; }

    [Column("is_read")]
    public bool? IsRead { get; set; }

    [Column("login_status")]
    public bool? LoginStatus { get; set; }

    [Column("user_name")]
    [StringLength(32)]
    public string? UserName { get; set; }

    [Column("pwd")]
    [StringLength(32)]
    public string? Pwd { get; set; }

    [Column("login_type")]
    public sbyte? LoginType { get; set; }

    [Column("service_name")]
    [StringLength(32)]
    public string? ServiceName { get; set; }
}
