using System.IO;
using Blog.Services.Log;

namespace blog.Middleware;

public class LogMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var actionLog = ActionLogService.GetActionLogService(context);
        
        // store original request body
        context.Request.EnableBuffering();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        // setting the request body of action log service
        actionLog.RequestBody = requestBody;
        actionLog.ShowRequest = true;
        context.Items["log"] = actionLog;
        
        // store original response body
        var originalResponseBody = context.Response.Body;
        
        try
        {
            using var responseBodyStream = new MemoryStream();
            // response body redirect to memory stream, so we can capture response content
            context.Response.Body = responseBodyStream;
            
            await next(context);
            
            // ensure response body can be read
            if (responseBodyStream.CanRead)
            {
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                
                // setting the response body of action log service
                actionLog.ShowResponse = true;
                actionLog.ResponseBody = responseBody;
                actionLog.IsMiddlewareSave(context);
                
                // copy response body to original response body
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalResponseBody);
            }
        }
        finally
        {
            // whatever happens, restore the original response body
            context.Response.Body = originalResponseBody;
        }
    }
}

public static class LogMiddlewareExtensions
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}