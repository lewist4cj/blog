using Blog.Api.Endpoints;
using Blog.Api.Middleware;
using Blog.Domain.Dtos;

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
