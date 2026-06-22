using Blog.Api.Infrastructure;

// Native AOT 下 ASP.NET Core MVC 模型元数据初始化
AppContext.SetSwitch("Microsoft.AspNetCore.Mvc.ApiExplorer.IsEnhancedModelMetadataSupported", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntry(builder.Configuration);
var app = builder.Build();

app.UseEntry();
app.Run();
