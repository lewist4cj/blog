using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Blog.Extensions.Config;
using Blog.Extensions.Filter;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace blog.Controllers;
public class SiteController(SiteMgr siteMgr) : BaseController
{
    [HttpGet("info")]
    public ApiResult SiteInfo(string name)
    { 
    //    var log =   ActionLogService.GetActionLogService(HttpContext);
    //    log.AddItemInfo("test_csharp_site", "test_csharp_val");
    //    log.Save(HttpContext);
        // redis.SetString("test_csharp_site", "test_csharp_val",TimeSpan.FromSeconds(30));
        var title = siteMgr.aboutSettings?.Bilibli;

        if (name == "site")
            return ApiResult.Success(siteMgr.siteSettings!);

        // check the role of the user. if the user is not admin, return failure.
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        _ = int.TryParse(role, out int userRoleValue);
        if (userRoleValue < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);

        switch (name)
        {
            case "title":
                return ApiResult.Success(title);
            case "logo":
                return ApiResult.Success(siteMgr.siteSettings?.Logo);
            case "beian":
                return ApiResult.Success(siteMgr.siteSettings?.BeiAn);
            case "isBlogMode":
                return ApiResult.Success(siteMgr.siteSettings?.IsBlogMode);
        }

       return ApiResult.Success();
    }
}