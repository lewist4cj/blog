using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Blog.Extensions.Filter;

public class AuthorizationFilterAttribute : AuthorizeAttribute
{
    private readonly RoleEnum _role;
    
    public AuthorizationFilterAttribute(RoleEnum Role = RoleEnum.Normal)
    {
        _role = Role;
    }
    
    public void OnAuthorization(AuthorizationFilterContext ctx)
    {
        var user = ctx.HttpContext.User;
        var claimsIdentity = (ClaimsIdentity?)user.Identity;
        
        if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
        {
            var res = ApiResult.Failure(Code.Forbidden);
            ctx.Result = new JsonResult(res); // 返回403
            return;
        }
    }
}