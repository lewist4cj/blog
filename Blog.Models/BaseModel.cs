using System.ComponentModel.DataAnnotations;

namespace blog.Models;

public class BaseModel
{
    [Key]
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}