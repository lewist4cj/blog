using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Blog.Extensions.Filter;

public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    private readonly RoleEnum _role;
    
    public AuthorizationFilterAttribute(RoleEnum Role = RoleEnum.Admin)
    {
        _role = Role;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var claimsIdentity = (ClaimsIdentity?)user.Identity;
        
        if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
        {
            var res = ApiResult.Failure(Code.Forbidden);
            context.Result = new JsonResult(res) { StatusCode = 403 }; 
            return;
        }

        var roleClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Role");

        if (roleClaim == null)
        {
            var res = ApiResult.Failure(Code.Forbidden);
            context.Result = new JsonResult(res) { StatusCode = 403 }; // 返回403
            return;
        }

        if (!int.TryParse(roleClaim.Value, out int userRoleValue))
        {
            var res = ApiResult.Failure(Code.Forbidden);
            context.Result = new JsonResult(res) { StatusCode = 403 }; // 返回403
            return;
        }

        // check if user role is sufficient
        if (userRoleValue < (int)_role)
        {
            var res = ApiResult.Failure(Code.Forbidden);
            context.Result = new JsonResult(res) { StatusCode = 403 }; // 返回403
            return;
        }

        // set user role to HttpContext
        context.HttpContext.Items["Role"] = roleClaim.Value;
    }
}