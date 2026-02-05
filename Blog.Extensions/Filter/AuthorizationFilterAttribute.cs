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
        
        // ensure user is authenticated
        if (!user.Identity!.IsAuthenticated)
        {
            var res = ApiResult.Failure(Code.Unauthorized);
            context.Result = new JsonResult(res) { StatusCode = 401 }; 
            return;
        }

        // get the ClaimsIdentity
        var claimsIdentity = user.Identity as ClaimsIdentity;
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
            context.Result = new JsonResult(res) { StatusCode = 403 }; 
            return;
        }

        // try parse the role value
        if (!int.TryParse(roleClaim.Value, out int userRoleValue))
        {
            var res = ApiResult.Failure(Code.Forbidden);
            context.Result = new JsonResult(res) { StatusCode = 403 }; 
            return;
        }

        // check if the user has the required role
        if (userRoleValue < (int)_role)
        {
            var res = ApiResult.Failure(Code.Forbidden);
            context.Result = new JsonResult(res) { StatusCode = 403 }; 
            return;
        }

        // setting the role to HttpContext
        context.HttpContext.Items["Role"] = roleClaim.Value;
    }
}