using Blog.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntry(builder.Configuration);
var app = builder.Build();

app.UseEntry();
app.Run();
