using Blog.Common;
using blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Core.DbContext;

public class BlogDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<LogModel> Logs { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (AppSettings.Configuration != null)
            {
                var connectionString = AppSettings.Configuration.GetConnectionString("DefaultConnection");
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
        }
    }
}