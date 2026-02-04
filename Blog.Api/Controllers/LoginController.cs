using Blog.Common;
using Blog.Common.TokenModule;
using Blog.Common.TokenModule.Models;
using Blog.Common.Utils;
using Blog.Domain;
using Blog.Services.Log;
using Blog.Services.UserApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace blog.Controllers;

public class LoginController(RuntimeLogService runtimeLogService,
 IUserService userService, ILogger<LoginController> logger) : BaseController
{

    [HttpPost]
    public async Task<ApiResult> CheckLogin(UserModelLoginDto dto)
    {
        runtimeLogService.AddItemNowTime();
        runtimeLogService.AddItemInfo("login info", $"username: {dto.Username},password:{dto.Password}");
        runtimeLogService.Save("LoginController/CheckLogin");

        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return ApiResult.Failure(Code.ValidationFailed);
        }
        if (!await userService.CheckPwd(dto))
        {
            logger.LogInformation("the username or password is incorrect");

            return ApiResult.Failure(Code.InvalidCredentials);
        }
        var user = await userService.GetUserAsync(dto);
        if (user == null)
        {
            logger.LogInformation($"the user {dto.Username} does not exist");
            return ApiResult.Failure(Code.InvalidCredentials);
        }

        var token = GetToken(user);
        return ApiResult.Success(token);
    }

    private string GetToken(UserModel user)
    {
        var jwtSection = AppSettings.Configuration!.GetSection("Jwt");
        var tokenModel = jwtSection.Get<JwtTokenModel>()!;
        tokenModel.ValidateAndReturn();
        tokenModel.Id = user.Id;
        tokenModel.UserName = user.Username;
        tokenModel.NickName = user.Nickname;
        tokenModel.Role = user.Role;
        var token = TokenHepler.GenerateToken(tokenModel);

        return token;
    }
}