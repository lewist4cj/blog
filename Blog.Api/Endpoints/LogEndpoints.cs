using Blog.Api.Filters;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Blog.Services.LogMgrApp;

namespace Blog.Api.Endpoints;

public static class LogEndpoints
{
    public static RouteGroupBuilder MapLogEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/list", GetLogModels)
            .RequireAuthorization()
            .AddEndpointFilter(new RoleAuthorizationFilter(RoleEnum.SuperAdmin));
        return group;
    }

    private static async Task<ApiResult> GetLogModels(int pageIndex, int pageSize, ILogService logService)
    {
        var res = await logService.GetLogModelsAsync(pageIndex, pageSize);
        return ApiResult.Success(res);
    }
}
