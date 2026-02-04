using Blog.Common.Utils;
using Blog.Services.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Authorize(Roles = "3" ,AuthenticationSchemes = "Bearer")]
public class SiteController : BaseController
{
    // private readonly ActionLogService _actionLogService = actionLogService;

    [HttpGet]
    public ApiResult SiteInfo()
    { 
       var log =   ActionLogService.GetActionLogService(HttpContext);
       log.AddItemInfo("test_csharp_site", "test_csharp_val");
       log.Save(HttpContext);
       return ApiResult.Success();
    }
}