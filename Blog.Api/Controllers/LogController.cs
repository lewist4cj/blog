using Blog.Common.Utils;
using Blog.Core.Repository;
using Blog.Domain;
using Blog.Domain.enums;
using Blog.Extensions.Filter;
using Blog.Services.LogMgrApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Authorize(AuthenticationSchemes = "Bearer")]
[AuthorizationFilter(RoleEnum.SuperAdmin)]  // Only allow SuperAdmin access
public class LogController(ILogService logService): BaseController 
{
    [HttpGet("list")]
    public async Task<ApiResult> GetLogModels(int pageIndex, int pageSize)
    {
       var res = await logService.GetLogModelsAsync(pageIndex, pageSize);
      
       return  ApiResult.Success(res);
    }
    
}