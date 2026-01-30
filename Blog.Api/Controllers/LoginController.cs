using blog.Utils;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController:BaseController
{
    [HttpGet]
    public IActionResult Login()
    {
        
        return Ok(new ApiResult<string>
        {
            Code = ApiResultCode.Success,
            Message = "登录成功",
            Data = "登录成功"
        });
    }
}