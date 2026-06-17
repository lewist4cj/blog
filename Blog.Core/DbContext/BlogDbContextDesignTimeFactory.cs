using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Blog.Core.DbContext;

/// <summary>
/// Design-time factory for EF Core migrations (used by `dotnet ef migrations add`)
/// </summary>
public class BlogDbContextDesignTimeFactory : IDesignTimeDbContextFactory<BlogDbContext>
{
    public BlogDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Blog.Api"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("Blog.Core");
        });

        return new BlogDbContext(optionsBuilder.Options,
            Microsoft.Extensions.Logging.Abstractions.NullLogger<BlogDbContext>.Instance);
    }
}
