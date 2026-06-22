using Blog.Api.Endpoints;
using Blog.Api.Middleware;
using Blog.Core.SqlSugar;
using SqlSugar;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationExt
{
    public static WebApplication UseEntry(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<LogMiddleware>();
        app.UseStaticFiles();

        // CodeFirst 自动同步数据库表结构
        try
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                SqlSugarSetup.InitDatabase(db);
            }
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "数据库同步跳过（PostgreSQL 未就绪）");
        }

        // Minimal API endpoints
        var api = app.MapGroup("/api");
        api.MapGroup("/user").MapUserEndpoints();
        api.MapGroup("/article").MapArticleEndpoints();
        api.MapGroup("/category").MapCategoryEndpoints();
        api.MapGroup("/collect").MapCollectEndpoints();
        api.MapGroup("/comment").MapCommentEndpoints();
        api.MapGroup("/site").MapSiteEndpoints();
        api.MapGroup("/log").MapLogEndpoints();
        api.MapGroup("/Upload").MapUploadEndpoints();

        return app;
    }
}
