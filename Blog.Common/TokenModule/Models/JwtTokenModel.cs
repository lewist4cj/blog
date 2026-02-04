using System.ComponentModel.DataAnnotations;

namespace Blog.Common.TokenModule.Models;

public class JwtTokenModel : IValidatableObject
{
    /// <summary>
    /// 发行人
    /// </summary>
    [Required(ErrorMessage = "Jwt Issuer is required.")]
    public string? Issuer { get; set; }

    /// <summary>
    /// 听众
    /// </summary>
    [Required(ErrorMessage = "Jwt Audience is required.")]
    public string?  Audience { get; set; } = "";

    /// <summary>
    /// 过期时间
    /// </summary>
    public int Expires { get; set; } = 10;

    /// <summary>
    /// 安全密钥
    /// </summary>
    [Required(ErrorMessage = "Jwt Security key is required.")]
    public string?  Security { get; set; } = "";

    /// <summary>
    /// ID
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string?  UserName { get; set; }
    
    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }
    
    /// <summary>
    /// 用户角色
    /// </summary>
    public sbyte? Role { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        
        // 验证Expire值的合理性
        if (Expires <= 0)
        {
            results.Add(new ValidationResult("Expire value must be greater than 0.", [nameof(Expires)]));
        }
        
        return results;
    }
}