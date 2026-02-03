using blog.Extensions;
using blog.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntry();
var app = builder.Build();

// 注册全局异常处理中间件

app.UseEntry();
app.Run();