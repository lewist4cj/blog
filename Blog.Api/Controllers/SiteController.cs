using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Blog.Extensions.Config;
using Blog.Extensions.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Authorize(AuthenticationSchemes = "Bearer")]
public class SiteController(SiteMgr siteMgr, OtherSiteMgr otherSiteMgr) : BaseController 
{
    [HttpGet("info")]
    public ApiResult SiteInfo(string name)
    {
        if (name == "site")
            return ApiResult.Success(siteMgr.siteSettings!);

        // check the role of the user. if the user is not admin, return failure.
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        _ = int.TryParse(role, out int userRoleValue);
        if (userRoleValue < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);

        switch (name)
        {
            case "email":
                var email = otherSiteMgr.emailSettings!;
                return ApiResult.Success(email);
            case "qq":
                var qqSettings = otherSiteMgr.qqSettings!;
                return ApiResult.Success(qqSettings);
            case "qiNiu":
                var beiAn = otherSiteMgr.qiNiuSettings!;
                return ApiResult.Success(beiAn);
            case "ai":
                var ai = otherSiteMgr.aiSettings!;
                return ApiResult.Success(ai);
            default:
                return ApiResult.Failure(Code.NotFound);
        }
    }

    [HttpGet("redirection")]
    public ApiResult Redirection()
    {
        var qqSettings = otherSiteMgr.qqSettings;
        var validationResult = qqSettings.Validate();
        
        // 如果验证失败，直接返回错误
        if (validationResult.Code != 200)
        {
            return validationResult;
        }
        
        var url = qqSettings.GetRedirectUrl();
        return ApiResult.Success(url);
    }
}