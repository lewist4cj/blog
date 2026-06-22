using System.Text;
using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.JsonContext;
using Blog.Api.JsonContext;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Blog.Common.TokenModule.Models;
using Blog.Common.RedisModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Blog.Services.Local;
using Blog.Services.Log;
using Blog.Core.SqlSugar;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization.Metadata;
using Blog.Common.Database;
using Blog.Core.Sync;
using Blog.Core.UnitOfWork;
using SqlSugar;

namespace Blog.Api.Infrastructure;

public static class WebApplicationBuilderExt
{
    public static IServiceCollection AddEntry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new AppSettings(configuration));
        services.AddSerilog(configuration);
        services.AddJwtAuthentication(configuration);
        services.AddRegister();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    private static void AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        string serilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);

        var logPath = configuration["Serilog:Path"] ?? "Logs";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: serilogOutputTemplate)
            .WriteTo.File(logPath,
                rollingInterval: RollingInterval.Day,
                outputTemplate: serilogOutputTemplate,
                retainedFileCountLimit: 31,
                retainedFileTimeLimit: TimeSpan.FromDays(2),
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 52428800
            )
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger, dispose: true);
        });
    }

    private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // 注册 JwtTokenModel 为 IOptions 以便运行时注入
        services.Configure<JwtTokenModel>(configuration.GetSection("Jwt"));

        // AOT 下 JwtTokenModel 类型被直接属性访问保留，Bind 安全
        var jwtSection = configuration.GetSection("Jwt");
        var tokenModel = new JwtTokenModel();
#pragma warning disable IL2026 // 类型通过 Startup 中直接属性访问保留
        jwtSection.Bind(tokenModel);
#pragma warning restore IL2026
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                var appEnvironment = configuration["AppEnvironment"] ?? "Production";
                opt.RequireHttpsMetadata = appEnvironment != "Development";
                opt.TokenValidationParameters = new()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenModel.Security!)),
                    ValidIssuer = tokenModel.Issuer,
                    ValidAudience = tokenModel.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };

                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async ctx =>
                    {
                        await HandleTokenBlacklistCheck(ctx);
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
                        await WriteJsonResponse(ctx.Response, ApiResult.Failure(Code.Forbidden), StatusCodes.Status403Forbidden);
                    },
                };
            });
    }

    private static async Task HandleTokenBlacklistCheck(MessageReceivedContext ctx)
    {
        var token = ctx.Request.Headers.Authorization.FirstOrDefault();
        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
        {
            var actualToken = token["Bearer ".Length..].Trim();
            var redisWorker = ctx.HttpContext.RequestServices.GetRequiredService<IRedisWorker>();
            var redisBlack = redisWorker.GetBlackString(actualToken);
            if (redisBlack != null)
            {
                var errorCode = redisBlack switch
                {
                    "User" => Code.UserBlackEnumType,
                    "Admin" => Code.AdminBlackEnumType,
                    "Device" => Code.DeviceBackEnumType,
                    _ => Code.UserBlackEnumType
                };

                await WriteJsonResponse(ctx.Response, ApiResult.Failure(errorCode), StatusCodes.Status200OK);
                ctx.Fail("");
            }
        }
    }

    private static async Task WriteJsonResponse(HttpResponse response, ApiResult result, int statusCode)
    {
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(result, typeof(ApiResult), DomainJsonContext.Default);
        await response.WriteAsync(json);
    }

    private static void AddRegister(this IServiceCollection services)
    {
        // Minimal API JSON 序列化配置（使用源码生成上下文，AOT 兼容）
        services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.TypeInfoResolver = JsonTypeInfoResolver.Combine(
                DomainJsonContext.Default, AppJsonContext.Default);
            opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opts.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            opts.SerializerOptions.PropertyNameCaseInsensitive = true;
            opts.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            opts.SerializerOptions.WriteIndented = true;
        });

        services.AddAuthorization();
        services.AddHttpContextAccessor();

        services.AddSingleton<RedisCore>();
        services.AddRepositoryRegister();
        services.AddServiceRegister();
        services.AddSingleton<LocalService>();

        services.AddScoped<ActionLogService>();
        services.AddScoped<RuntimeLogService>();

        // 注册 SqlSugarClient（替代 EF Core DbContext）
        services.AddSingleton<ISqlSugarClient>(sp =>
        {
            var settings = sp.GetRequiredService<DatabaseSettings>();
            return SqlSugarSetup.CreateClient(settings);
        });

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var settings = new DatabaseSettings();
            config.GetSection("Database").Bind(settings);
            if (string.IsNullOrEmpty(settings.DefaultConnection))
                settings.DefaultConnection = config.GetConnectionString("DefaultConnection")!;
            return settings;
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IElasticsearchSyncService, ElasticsearchSyncService>();
        services.AddHostedService<CanalSyncService>();
    }
}
