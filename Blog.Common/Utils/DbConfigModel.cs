using System.ComponentModel.DataAnnotations;

namespace Blog.Common.Utils;

public class DbConfigModel : IValidatableObject
{
    [Required(ErrorMessage = "Database connection string is required.")]
    public string? ConnectionString { get; set; }
    
    [Required(ErrorMessage = "Database provider is required.")]
    public string? Provider { get; set; }
    
    public int CommandTimeout { get; set; } = 30;
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        
        // 验证CommandTimeout的合理性
        if (CommandTimeout <= 0)
        {
            results.Add(new ValidationResult("Command timeout must be greater than 0 seconds.", [nameof(CommandTimeout)]));
        }
        
        return results;
    }
}