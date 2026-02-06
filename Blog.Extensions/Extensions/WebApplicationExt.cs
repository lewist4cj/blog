using Microsoft.AspNetCore.Builder;
using Blog.Extensions.Middleware;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationExt
{
    public static WebApplication UseEntry(this WebApplication app)
    {
        // GlobalExceptionMiddleware - Must be placed before UseMiddleware<LogMiddleware>
        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<LogMiddleware>();
        app.MapControllers();

        return app;
    }
}