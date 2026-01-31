using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

[Table("global_notications")]  // 修正表名以匹配原始SQL
public class GlobalNotificationModel : BaseModel
{
    [MaxLength(32)]
    public string? Title { get; set; }
    
    [MaxLength(256)]
    public string? Icon { get; set; }
    
    [MaxLength(64)]
    public string? Content { get; set; }
    
    [MaxLength(256)]
    public string? Href { get; set; }
}