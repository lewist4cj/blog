using System.ComponentModel.DataAnnotations;

namespace Blog.Services.UserApp;

public class UserModelLoginDto
{
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "用户名长度必须在1-50个字符之间")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "密码长度必须在1-100个字符之间")]
    public string Password { get; set; } = string.Empty;
}