using Blog.Common;
using Blog.Core.BlogDbContext;
using blog.Middleware;
using Blog.Services.Log;
using Blog.Services.Local;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//  Configuration
var appSettings = new AppSettings(builder.Configuration);
builder.Services.AddSingleton(appSettings);
// 确保AppSettings.Configuration被正确设置
AppSettings.Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// 添加 IHttpContextAccessor 服务
builder.Services.AddHttpContextAccessor();

// 配置数据库上下文
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 遵循EF Core性能优化配置标准
builder.Services.AddDbContext<BlogDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           // 默认不跟踪查询结果，提高性能
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddSingleton<LocalService>();

// 注册日志服务
builder.Services.AddScoped<ActionLogService>();
builder.Services.AddScoped<RuntimeLogService>();

var app = builder.Build();


app.UseHttpsRedirection();

// 注册日志中间件 - 应该在 UseAuthorization 之前注册
app.UseMiddleware<LogMiddleware>();

// app.UseAuthorization();

app.MapControllers();

app.Run();