using System.Text;
using Blog.Common;
using Blog.Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Blog.Common.TokenModule.Models;
using Blog.Common.RedisModule;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Blog.Services;
using Blog.Services.Local;
using Blog.Services.Log;
using Blog.Core.DbContext;
using Blog.Extensions.Filter;
using Serilog;
using Microsoft.Extensions.Logging;
using Blog.Extensions.Config;

namespace Blog.Extensions;

public static class WebApplicationBuilderExt
{
    public static IServiceCollection AddEntry(this IServiceCollection services)
    {
        services.AddTransient(typeof(SiteMgr),typeof(SiteMgr));
        services.AddTransient(typeof(OtherSiteMgr),typeof(OtherSiteMgr));
        // Appsettings Register 
        services.AddSingleton(new AppSettings(services.GetConfiguration()));
        // initialize serilog configuration
        services.AddSerilog();
        // Jwt Authentication configuration 
        services.AddJwtAuthentication();
        // Service Register
        services.AddRegister();
        return services;
    }

    private static void AddSerilog(this IServiceCollection services)
    {
        string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);

        var logPath = AppSettings.app("Serilogs","Path") ?? "Logs";
      
        // var logDirectory = Path.Combine(Directory.GetCurrentDirectory(),logPath);
        // if (!Directory.Exists(logDirectory))
        // {
        //     Directory.CreateDirectory(logDirectory);
        // }

        // var logFilePath = Path.Combine(logDirectory, "log_.log");
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: SerilogOutputTemplate)
            .WriteTo.File(logPath,
                rollingInterval: RollingInterval.Day,
                outputTemplate: SerilogOutputTemplate,
                retainedFileCountLimit: 31,
                retainedFileTimeLimit: TimeSpan.FromDays(2),
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 52428800 // 50MB
            )
            .CreateLogger();

        // Serilog Register
        services.AddLogging(builder =>
        {
            builder.ClearProviders(); // clear other logger
            builder.AddSerilog(Log.Logger, dispose: true);
        });

    }

    private static void AddJwtAuthentication(this IServiceCollection services)
    {
        var tokenModel = AppSettings.Configuration?.GetSection("Jwt").Get<JwtTokenModel>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                var appEnvironment = AppSettings.Configuration!["AppEnvironment"] ?? "Production";
                opt.RequireHttpsMetadata = appEnvironment != "Development";
                opt.TokenValidationParameters = new()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenModel?.Security!)),
                    ValidIssuer = tokenModel?.Issuer,
                    ValidAudience = tokenModel?.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };

                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async ctx =>
                    {
                        await HandleTokenBlacklistCheck(ctx, services);
                    },
                    OnAuthenticationFailed = async ctx =>
                    {
                        if (ctx.Exception is SecurityTokenExpiredException)
                        {
                            await WriteJsonResponse(ctx.Response, ApiResult.Failure(Code.TokenExpired), StatusCodes.Status401Unauthorized);
                            return;
                        }
                        
                        await WriteJsonResponse(ctx.Response, ApiResult.Failure(Code.Unauthorized), StatusCodes.Status401Unauthorized);
                    },

                    OnChallenge = ctx =>
                    {
                        ctx.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnForbidden = async ctx =>
                    {
                        // the client will get 403 Forbidden if the user does not have permission
                        await WriteJsonResponse(ctx.Response, ApiResult.Failure(Code.Forbidden), StatusCodes.Status403Forbidden);
                    },
                };
            });
    }

    /// <summary>
    /// Checks if the token is in the blacklist and handles the response accordingly
    /// </summary>
    /// <param name="ctx">The message received context</param>
    /// <param name="services">The service collection</param>
    private static async Task HandleTokenBlacklistCheck(MessageReceivedContext ctx, IServiceCollection services)
    {
        // Check token presence in the blacklist.
        var token = ctx.Request.Headers.Authorization.FirstOrDefault();
        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
        {
            var actualToken = token["Bearer ".Length..].Trim();
            var redisWorker = services.BuildServiceProvider().GetRequiredService<IRedisWorker>();
            var redisBlack = redisWorker.GetBlackString(actualToken);
            if (redisBlack != null)
            {
                // Determine the appropriate error code based on blacklist type
                var errorCode = redisBlack switch
                {
                    "User" => Code.UserBlackEnumType,
                    "Admin" => Code.AdminBlackEnumType,
                    "Device" => Code.DeviceBackEnumType,
                    _ => Code.UserBlackEnumType
                };

                // Return the error response
                await WriteJsonResponse(ctx.Response, ApiResult.Failure(errorCode), StatusCodes.Status200OK);
                ctx.Fail("");
                return;
            }
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Writes a JSON response with the specified status code
    /// </summary>
    /// <param name="response">The HTTP response</param>
    /// <param name="result">The API result to serialize</param>
    /// <param name="statusCode">The HTTP status code</param>
    private static async Task WriteJsonResponse(HttpResponse response, ApiResult result, int statusCode)
    {
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(result);
        await response.WriteAsync(json);
    }

    private static void AddRegister(this IServiceCollection services)
    {
        services.AddControllers(opts =>
       {
           opts.Filters.Add<ValidateModelFilter.ValidateModelAttribute>();
       })
       .ConfigureApiBehaviorOptions(options =>
       {
           options.SuppressModelStateInvalidFilter = true;
       })
       .AddJsonOptions(opts =>
       {
           opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
           opts.JsonSerializerOptions.WriteIndented = false;
       });

        services.AddHttpContextAccessor();

        // Note: Current version is 12.0. For version 13.0+, params should accept Type[] array
        services.AddAutoMapper(typeof(BlogProfile));
        services.AddSingleton<RedisCore>();
        services.AddRepositoryRegister();
        services.AddServiceRegister();
        services.AddSingleton<LocalService>();

        services.AddScoped<ActionLogService>();
        services.AddScoped<RuntimeLogService>();
        services.AddDbContext<BlogDbContext>(opt =>
        {
            if (AppSettings.Configuration == null) return;
            var connectionString = AppSettings.Configuration.GetConnectionString("DefaultConnection");
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                // disable QueryTrackingBehavior to improve performance
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }
    private static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetService<IConfiguration>()!;
        return config;
    }



}