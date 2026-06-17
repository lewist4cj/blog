using Blog.Services.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Blog.Api.Middleware;

public class LogMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var actionLog = ActionLogService.GetActionLogService(context);

        context.Request.EnableBuffering();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        actionLog.RequestBody = requestBody;
        actionLog.ShowRequest = true;
        context.Items["log"] = actionLog;

        var originalResponseBody = context.Response.Body;

        try
        {
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await next(context);

            if (responseBodyStream.CanRead)
            {
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                actionLog.ShowResponse = true;
                actionLog.ResponseBody = responseBody;
                actionLog.IsMiddlewareSave(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalResponseBody);
            }
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }
    }
}
