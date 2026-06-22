using Blog.Common;
using Blog.Common.TokenModule;
using Blog.Common.TokenModule.Models;
using Blog.Common.Utils;
using Blog.Domain;
using Blog.Extensions.Validation;
using Blog.Services.Log;
using Blog.Services.UserApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Api.Controllers;

public class UserController(RuntimeLogService runtimeLogService,
    IUserService userService, ILogger<UserController> logger, IRedisWorker redisWorker,
    IOptions<JwtTokenModel> jwtOptions) : BaseController
{

    [HttpPost("login")]
    public async Task<ApiResult> CheckLogin(UserModelLoginDto dto)
    {
        runtimeLogService.AddItemNowTime();
        runtimeLogService.AddItemInfo("login info", $"username: {dto.Username}");
        runtimeLogService.Save("UserController/CheckLogin");

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


    [HttpGet("unregister")]
    public async Task<ApiResult> Unregister()
    {
        // get the token from the request header and add it to the blacklist
        var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return ApiResult.Failure(Code.BadRequest);
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var tokenHandler = TokenHepler.GetSecurityToken(token, jwtOptions.Value);

        // if the remaining time is less than or equal to zero, then the token has expired. so we dot need to add it to the blacklist.
        var remainingTime = tokenHandler.ValidTo - DateTime.UtcNow;
        if (remainingTime <= TimeSpan.Zero)
        {
            return ApiResult.Success("Unregistered successfully.");
        }
        // add the token to the blacklist, because the token has not expired.
        redisWorker.SetBlackString(token, TokenBlackEnum.User, remainingTime);

        return ApiResult.Success("Unregistered successfully.");
    }
    private string GetToken(UserModel user)
    {
        var tokenModel = jwtOptions.Value;
        tokenModel.Validate();
        tokenModel.Id = user.Id;
        tokenModel.UserName = user.Username;
        tokenModel.NickName = user.Nickname;
        tokenModel.Role = user.Role;
        var token = TokenHepler.GenerateToken(tokenModel);

        return token;
    }


}
