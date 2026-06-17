using System.Text;
using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.Config;
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
using Blog.Services.ConfigMgrApp;
using Blog.Core.DbContext;
using Blog.Api.Filter;
using Serilog;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Blog.Common.Database;
using Blog.Core.Sync;
using Blog.Core.UnitOfWork;

namespace Blog.Api.Infrastructure;

public static class WebApplicationBuilderExt
{
    public static IServiceCollection AddEntry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new AppSettings(configuration));
        services.AddSerilog(configuration);
        services.AddJwtAuthentication(configuration);
        services.AddRegister(configuration);

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
        string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);

        var logPath = configuration["Serilog:Path"] ?? "Logs";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: SerilogOutputTemplate)
            .WriteTo.File(logPath,
                rollingInterval: RollingInterval.Day,
                outputTemplate: SerilogOutputTemplate,
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
        var tokenModel = configuration.GetSection("Jwt").Get<JwtTokenModel>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                var appEnvironment = configuration["AppEnvironment"] ?? "Production";
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
                return;
            }
        }
    }

    private static async Task WriteJsonResponse(HttpResponse response, ApiResult result, int statusCode)
    {
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(result);
        await response.WriteAsync(json);
    }

    private static void AddRegister(this IServiceCollection services, IConfiguration configuration)
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
            opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            opts.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opts.JsonSerializerOptions.WriteIndented = true;
        });

        services.AddHttpContextAccessor();

        services.AddAutoMapper(typeof(BlogProfile));
        services.AddSingleton<RedisCore>();
        services.AddRepositoryRegister();
        services.AddServiceRegister(configuration);
        services.AddSingleton<LocalService>();

        services.AddScoped<ActionLogService>();
        services.AddScoped<RuntimeLogService>();
        services.AddScoped<DbContextFactory>();

        services.AddDbContext<BlogDbContext>((sp, opt) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            DbContextFactory.ConfigureWriteContext(opt, config);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<ReadOnlyDbContext>(sp =>
        {
            var factory = sp.GetRequiredService<DbContextFactory>();
            return new ReadOnlyDbContext(factory.CreateReadContext());
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
        services.AddScoped<ISiteConfigService, SiteConfigService>();
        services.AddSingleton<IElasticsearchSyncService, ElasticsearchSyncService>();
        services.AddHostedService<CanalSyncService>();
    }
}
