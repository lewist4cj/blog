using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blog.Common;
using Blog.Common.TokenModule;
using Blog.Common.TokenModule.Models;
using Blog.Common.Utils;
using Blog.Domain;
using Blog.Extensions.Validation;
using Blog.Services.Log;
using Blog.Services.UserApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace blog.Controllers;

public class UserController(RuntimeLogService runtimeLogService,
 IUserService userService, ILogger<UserController> logger, IRedisWorker redisWorker) : BaseController
{

    [HttpPost("login")]
    public async Task<ApiResult> CheckLogin(UserModelLoginDto dto)
    {
        runtimeLogService.AddItemNowTime();
        runtimeLogService.AddItemInfo("login info", $"username: {dto.Username},password:{dto.Password}");
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
        // 获取请求中的token数据，并将token加入黑名单中
        var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return ApiResult.Failure(Code.BadRequest);
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var tokenHandler = TokenHepler.GetSecurityToken(token);
        
        // 计算从当前时间到令牌过期时间的剩余时间
        var remainingTime = tokenHandler.ValidTo - DateTime.UtcNow;
        if (remainingTime <= TimeSpan.Zero)
        {
            // 如果令牌已经过期，则不需要加入黑名单
            return ApiResult.Success("Unregistered successfully.");
        }
        
        redisWorker.SetBlackString(token, TokenBlackEnum.User, remainingTime);

        return ApiResult.Success("Unregistered successfully.");
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