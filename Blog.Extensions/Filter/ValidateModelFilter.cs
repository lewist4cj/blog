using Blog.Common;
using Blog.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.Extensions.Filter;

public abstract class ValidateModelFilter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // 构建错误详情
                var errors = new Dictionary<string, string[]>();
                foreach (var kvp in context.ModelState)
                {
                    errors[kvp.Key] = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray();
                }

                // 使用ApiResult返回统一响应，使用Code.BadRequest错误码
                var apiResult = ApiResult.Failure( "One or more validation errors occurred.", errors);
                
                context.Result = new JsonResult(apiResult)
                {
                    StatusCode = 400
                };
                // 这里如何直接短路
            }
        }
    }
}