using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Blog.Extensions.Filter;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[AuthorizationFilter(Role:RoleEnum.SuperAdmin,AuthenticationSchemes = "Bearer")]
public class SiteController(IRedisWorker redis) : BaseController
{
    [HttpGet]
    public ApiResult SiteInfo()
    { 
    //    var log =   ActionLogService.GetActionLogService(HttpContext);
    //    log.AddItemInfo("test_csharp_site", "test_csharp_val");
    //    log.Save(HttpContext);
        redis.SetString("test_csharp_site", "test_csharp_val",TimeSpan.FromSeconds(30));
       return ApiResult.Success();
    }
}