using Blog.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
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
    public class ValidationError
    {
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string Field { get; }
        public string Message { get; }
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }
}