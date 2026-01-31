using Blog.Common;
using blog.Middleware;
using Blog.Services.Log;
using Blog.Core.DbContext;
using Blog.Services.Local;

var builder = WebApplication.CreateBuilder(args);
//  Configuration
builder.Services.AddSingleton(new AppSettings(builder.Configuration));

// Add services to the container.
builder.Services.AddControllers();

// 添加 IHttpContextAccessor 服务
builder.Services.AddHttpContextAccessor();

// 配置数据库上下文
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BlogDbContext>();
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