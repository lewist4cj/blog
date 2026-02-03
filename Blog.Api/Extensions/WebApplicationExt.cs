using blog.Middleware;

namespace blog.Extensions;

public static class WebApplicationExt
{
    public static void UseEntry(this WebApplication app)
    {
        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseCors("any");
        app.UseLogMiddleware();
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMapControllers();
        
    }

    private static void UseMapControllers(this WebApplication app)
    {
        app.MapControllers();

    }
}