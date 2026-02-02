using Blog.Common.Utils;
using Blog.Services.Log;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController(RuntimeLogService runtimeLogService) : BaseController
{

    [HttpGet]
    public IActionResult Login()
    {
        runtimeLogService.AddItemInfo("RunTimeLog","ce");
        runtimeLogService.AddItemNowTime();
        runtimeLogService.Save("Login");
        
        return Ok(new ApiResult<string>
        {
            Code = ApiResultCode.Success,
            Message = "登录成功",
            Data = "登录成功"
        });
    }
}