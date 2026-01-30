using Blog.Common;
using blog.Middleware;
using Microsoft.EntityFrameworkCore;
using blog.Models;
using blog.Services.Log;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//  Configuration
builder.Services.AddSingleton(new AppSettings(builder.Configuration));

// Add services to the container.
builder.Services.AddControllers();


// serilog configuration 
if (AppSettings.Configuration != null)
{
    var logConfig = new LoggerConfiguration().ReadFrom.Configuration(AppSettings.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
    Log.Logger = logConfig.CreateLogger();
}

// 配置数据库上下文
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 注册日志服务
builder.Services.AddScoped<ActionLogService>();
builder.Services.AddScoped<RuntimeLogService>();

var app = builder.Build();


app.UseHttpsRedirection();

// 注册日志中间件 - 应该在 UseAuthorization 之前注册
app.UseMiddleware<LogMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();