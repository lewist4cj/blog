using System.Text;
using Blog.Common;
using Blog.Common.TokenModule.Models;
using Blog.Core.DbContext;
using Blog.Extensions;
using Blog.Filter;
using Blog.Services;
using Blog.Services.Local;
using Blog.Services.Log;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace blog.Extensions;

public static class WebApplicationBuilderExt
{
    public static IServiceCollection AddEntry(this IServiceCollection services)
    {
        // AppSettings Register 
        services.AddSingleton(new AppSettings(services.GetConfiguration()));
        #region JWT Authentication
        var tokenModel = AppSettings.Configuration?.GetSection("Jwt").Get<JwtTokenModel>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                // 从appsettings.json读取自定义环境配置
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
                        // 此处代码终止
                        context.HandleResponse();
                        var res = "{\"code\":203,\"error\":\"no authorization\"}";
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status203NonAuthoritative;
                        context.Response.WriteAsync(res);
                        return Task.FromResult(0);
                    }
                };
            });
        #endregion

        #region Add services register

        services.AddControllers(opts =>
        {
            opts.Filters.Add<ValidateModelFilter.ValidateModelAttribute>();
        }).AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            opts.JsonSerializerOptions.WriteIndented = false;
        });
        


        services.AddHttpContextAccessor();

        // Note: Current version is 12.0. For version 13.0+, params should accept Type[] array
        services.AddAutoMapper(typeof(BlogProfile));
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
        #endregion
        return services;
    }

    private static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetService<IConfiguration>()!;
        return config;
    }
    
}