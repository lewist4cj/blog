using System.ComponentModel.DataAnnotations;

namespace Blog.Common;

public class IocTagsConfig:IValidatableObject
{
    [Required(ErrorMessage = "Ioc Tags is required.")]
    public List<string>? List { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (List == null || List.Count == 0)
        {
            results.Add(new ValidationResult("the list can not be empty and the count must be greater than 0", [nameof(List)]));
        }

        return results;
    }
}