using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using blog.Models.enums.Log;

namespace blog.Models;

[Table("log_models")]
public class LogModel : BaseModel
{
    [Column("log_type")]
    public LogTypeEnum LogType { get; set; }
    
    [MaxLength(32)]
    public string? Title { get; set; }
    
    public string? Content { get; set; }
    
    public LogLevelEnum? Level { get; set; }
    
    [Column("user_id")]
    public long? UserId { get; set; }
    
    [MaxLength(32)]
    public string? Ip { get; set; }
    
    [MaxLength(64)]
    public string? Addr { get; set; }
    
    [Column("is_read")]
    public bool? IsRead { get; set; }
    
    [Column("login_status")]
    public bool? LoginStatus { get; set; }
    
    [Column("user_name")]
    [MaxLength(32)]
    public string? UserName { get; set; }
    
    [MaxLength(32)]
    public string? Pwd { get; set; }
    
    [Column("login_type")]
    public LoginTypeEnum? LoginType { get; set; }
    
    [Column("service_name")]
    [MaxLength(32)]
    public string? ServiceName { get; set; }
    
    // 导航属性
    [ForeignKey(nameof(UserId))]
    public UserModel? UserModel { get; set; }
}