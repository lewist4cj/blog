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

namespace Blog.Extensions;

public static class WebApplicationBuilderExt
{
    public static IServiceCollection AddEntry(this IServiceCollection services)
    {
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
        
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        
        var logFilePath = Path.Combine(logDirectory, "log_.log");
        
        // 使用 appsettings.json 配置，但要确保配置已经加载
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: SerilogOutputTemplate)
            .WriteTo.File(logFilePath,
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
            builder.ClearProviders(); // 清除其他日志提供者
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
                    ValidAudience = tokenModel?.Audience
                };

                opt.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        // if no unauthorized request, challenge to return 401
                        context.HandleResponse();
                        var res = JsonSerializer.Serialize(ApiResult.Failure(Code.Unauthorized));
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.WriteAsync(res);
                        return Task.FromResult(0);
                    },
                    OnForbidden = context =>
                    {
                        // the client will get 403 Forbidden if the user does not have permission
                        var res = JsonSerializer.Serialize(ApiResult.Failure(Code.Forbidden));
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.WriteAsync(res);
                        return Task.FromResult(0);
                    }
                };
            });
    }
    private static void AddRegister(this IServiceCollection services)
    {
        services.AddControllers(opts =>
       {
           opts.Filters.Add<ValidateModelFilter.ValidateModelAttribute>();
       }).AddJsonOptions(opts =>
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