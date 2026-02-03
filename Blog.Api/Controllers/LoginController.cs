using Blog.Common;
using Blog.Common.TokenModule;
using Blog.Common.TokenModule.Models;
using Blog.Common.Utils;
using Blog.Services.Log;
using Blog.Services.UserApp;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController(RuntimeLogService runtimeLogService, IUserService userService) : BaseController
{

    [HttpPost]
    public async Task<ApiResult> CheckLogin(UserModelLoginDto dto)
    {
        runtimeLogService.AddItemNowTime();
        runtimeLogService.AddItemInfo("login","check user login");
        runtimeLogService.Save("LoginController/CheckLogin");
        
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return ApiResult.Failure(Code.ValidationFailed);
        }
        if (!await userService.CheckPwd(dto))
        {
            return ApiResult.Failure(Code.InvalidCredentials);
        }

        var token = GetToken();
        return ApiResult.Success(token);
    }

    public string GetToken()
    {
        var token = AppSettings.Configuration.GetSection("Jwt").Get<JwtTokenModel>();
        return TokenHepler.GenerateToken(token);
    }
}