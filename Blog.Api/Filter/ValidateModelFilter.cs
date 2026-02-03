using Blog.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json.Serialization;
namespace Blog.Filter;

public abstract class ValidateModelFilter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key]!.Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
                    
                // 使用ApiResult类型返回统一响应
                var apiResult = new ApiResult(400u, "binding error", errors);
                
                context.Result = new JsonResult(apiResult)
                {
                    StatusCode = 400
                };
            }
        }
    }
    public class ValidationError(string field, string message)
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Field { get; } = field != string.Empty ? field : null;
        public string Message { get; } = message;
    }
}