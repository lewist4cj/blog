using Blog.Common;
using Blog.Common.TokenModule;
using Blog.Common.TokenModule.Models;
using Blog.Common.Utils;
using Blog.Domain;
using Blog.Extensions.Validation;
using Blog.Services.Log;
using Blog.Services.UserApp;
using Microsoft.Extensions.Options;

namespace Blog.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/login", Login);
        group.MapGet("/unregister", Unregister).RequireAuthorization();
        return group;
    }

    private static async Task<ApiResult> Login(
        UserModelLoginDto dto,
        RuntimeLogService runtimeLogService,
        IUserService userService,
        ILogger<Program> logger,
        IOptions<JwtTokenModel> jwtOptions)
    {
        runtimeLogService.AddItemNowTime();
        runtimeLogService.AddItemInfo("login info", $"username: {dto.Username}");
        runtimeLogService.Save("UserEndpoints/Login");

        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return ApiResult.Failure(Code.ValidationFailed);

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

        var token = GetToken(user, jwtOptions);
        return ApiResult.Success(token);
    }

    private static async Task<ApiResult> Unregister(
        HttpContext httpContext,
        IRedisWorker redisWorker,
        IOptions<JwtTokenModel> jwtOptions)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return ApiResult.Failure(Code.BadRequest);

        var token = authHeader["Bearer ".Length..].Trim();
        var tokenHandler = TokenHepler.GetSecurityToken(token, jwtOptions.Value);

        var remainingTime = tokenHandler.ValidTo - DateTime.UtcNow;
        if (remainingTime <= TimeSpan.Zero)
            return ApiResult.Success("Unregistered successfully.");

        redisWorker.SetBlackString(token, TokenBlackEnum.User, remainingTime);
        return ApiResult.Success("Unregistered successfully.");
    }

    private static string GetToken(UserModel user, IOptions<JwtTokenModel> jwtOptions)
    {
        var tokenModel = jwtOptions.Value;
        tokenModel.Validate();
        tokenModel.Id = user.Id;
        tokenModel.UserName = user.Username;
        tokenModel.NickName = user.Nickname;
        tokenModel.Role = user.Role;
        return TokenHepler.GenerateToken(tokenModel);
    }
}
