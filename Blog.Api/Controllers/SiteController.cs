using Blog.Common.Utils;
using Blog.Services.Log;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class SiteController(ActionLogService actionLogService) : ControllerBase
{
    // private readonly ActionLogService _actionLogService = actionLogService;

    [HttpGet]
    public ApiResult SiteInfo()
    { 
       var log =   ActionLogService.GetActionLogService(HttpContext);
       log.AddItemInfo("test_csharp_site", "test_csharp_val");
       log.Insert(HttpContext);
       return ApiResult.Success();
    }
}