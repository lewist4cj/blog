using System.ComponentModel.DataAnnotations;

namespace Blog.Common.RedisModule;

public class RedisModle : IValidatableObject
{
    [Required(ErrorMessage = "Redis Host is required.")]
    public string? Host { get; set; }
    
    [Required(ErrorMessage = "Redis Port is required.")]
    public string? Port { get; set; }
    
    public string? Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        
        // 验证端口格式
        if (!string.IsNullOrEmpty(Port) && !ushort.TryParse(Port, out _))
        {
            results.Add(new ValidationResult("Redis Port must be a valid numeric value.", [nameof(Port)]));
        }
        
        return results;
    }
}