using Blog.Common;
using Blog.Common.TokenModule;
using Blog.Common.TokenModule.Models;
using Blog.Common.Utils;
using Blog.Domain;
using Blog.Services.Log;
using Blog.Services.UserApp;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
public class LoginController(RuntimeLogService runtimeLogService, IUserService userService) : BaseController
{

    [HttpPost]
    public async Task<ApiResult> CheckLogin(UserModelLoginDto dto)
    {
        runtimeLogService.AddItemNowTime();
        runtimeLogService.AddItemInfo("login info",$"username: {dto.Username},password:{dto.Password}");
        runtimeLogService.Save("LoginController/CheckLogin");
        
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return ApiResult.Failure(Code.ValidationFailed);
        }
        if (!await userService.CheckPwd(dto))
        {
            return ApiResult.Failure(Code.InvalidCredentials);
        }
        var user = await userService.GetUserAsync(dto);
        if (user == null)
        {
            return ApiResult.Failure(Code.InvalidCredentials);
        }

        var token = GetToken(user);
        return ApiResult.Success(token);
    }

    public string GetToken(UserModel user)
    {
        var jwtSection = AppSettings.Configuration!.GetSection("Jwt");
        var tokenModel = jwtSection.Get<JwtTokenModel>();
        
        if (tokenModel == null)
        {
            throw new InvalidOperationException("JWT configuration is missing from app settings.");
        }
        tokenModel.Id = user.Id;
        tokenModel.UserName = user.Username;
        tokenModel.NickName = user.Nickname;
        tokenModel.Role = user.Role;
        var token = TokenHepler.GenerateToken(tokenModel);

        return token;
    }
}