using Blog.Common;
using Blog.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Api.Filter;

public abstract class ValidateModelFilter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, string[]>();
                foreach (var kvp in context.ModelState)
                {
                    errors[kvp.Key] = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray();
                }

                var apiResult = ApiResult.Failure("One or more validation errors occurred.", errors);

                context.Result = new JsonResult(apiResult)
                {
                    StatusCode = 400
                };
            }
        }
    }
}
