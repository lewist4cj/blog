using Blog.Common.RespModule;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Blog.Common.Utils;
using Blog.Common;

namespace Blog.Extensions.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiResult result;
            int statusCode;

            // 根据异常类型返回不同的错误信息
            switch (exception)
            {
                case JsonException:
                    result = ApiResult.Failure(Code.BadRequest, "Request data format error: " + exception.Message);
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case InvalidOperationException:
                    result = ApiResult.Failure(Code.BadRequest, exception.Message);
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case ArgumentException:
                    result = ApiResult.Failure(Code.BadRequest, exception.Message);
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                default:
                    result = ApiResult.Failure(Code.InternalServerError);
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var jsonResponse = JsonSerializer.Serialize(result);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}