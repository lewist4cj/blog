using Blog.Common.RespModule;
using Blog.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Blog.Common;
using Npgsql;
using StackExchange.Redis;

namespace Blog.Api.Middleware;

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

        var isDevelopment = string.Equals(
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            "Development",
            StringComparison.OrdinalIgnoreCase);

        switch (exception)
        {
            case JsonException:
                result = ApiResult.Failure(Code.BadRequest, isDevelopment
                    ? "Request data format error: " + exception.Message
                    : "Invalid request data format");
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case InvalidOperationException:
                result = ApiResult.Failure(Code.BadRequest, isDevelopment ? exception.Message : "Invalid operation");
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case ArgumentException:
                result = ApiResult.Failure(Code.BadRequest, isDevelopment ? exception.Message : "Invalid argument");
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case PostgresException:
                result = ApiResult.Failure(Code.DbAccessDenied, isDevelopment
                    ? exception.Message
                    : "Database access error");
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case RedisConnectionException:
                result = ApiResult.Failure(Code.RedisAccessDenied, isDevelopment
                    ? exception.Message
                    : "Cache service unavailable");
                statusCode = StatusCodes.Status400BadRequest;
                break;
            default:
                result = ApiResult.Failure(Code.InternalServerError, isDevelopment ? exception.Message : "An internal server error occurred");
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(result);
        await context.Response.WriteAsync(jsonResponse);
    }
}
