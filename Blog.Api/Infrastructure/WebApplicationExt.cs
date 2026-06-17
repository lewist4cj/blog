using Blog.Api.Middleware;

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
        app.MapControllers();
        app.UseStaticFiles();

        return app;
    }
}
