using System.Security.Claims;
using System.Text.Json;
using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Blog.Domain.JsonContext;

namespace Blog.Api.Filters;

/// <summary>
/// Minimal API 角色授权过滤器（替换旧的 AuthorizationFilterAttribute）
/// </summary>
public class RoleAuthorizationFilter(RoleEnum requiredRole) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated == true)
        {
            return Results.Json(ApiResult.Failure(Code.Unauthorized), DomainJsonContext.Default.ApiResult, statusCode: 401);
        }

        var claimsIdentity = user.Identity as ClaimsIdentity;
        if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
        {
            return Results.Json(ApiResult.Failure(Code.Forbidden), DomainJsonContext.Default.ApiResult, statusCode: 403);
        }

        var roleClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Role");
        if (roleClaim == null || !int.TryParse(roleClaim.Value, out int userRoleValue) || userRoleValue < (int)requiredRole)
        {
            return Results.Json(ApiResult.Failure(Code.Forbidden), DomainJsonContext.Default.ApiResult, statusCode: 403);
        }

        context.HttpContext.Items["Role"] = roleClaim.Value;
        return await next(context);
    }
}
