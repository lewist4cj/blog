using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Blog.Common;

namespace Blog.Core.DbContext;

public class BlogDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
{
    public BlogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        
        // 获取当前目录
        var currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"Current directory: {currentDirectory}");
        
        // 尝试不同的配置文件位置
        var configBuilder = new ConfigurationBuilder();
        
        // 检查当前目录
        if (File.Exists(Path.Combine(currentDirectory, "appsettings.json")))
        {
            configBuilder.SetBasePath(currentDirectory);
            Console.WriteLine("Found appsettings.json in current directory");
        }
        else
        {
            // 尝试向上查找解决方案根目录
            var solutionRoot = FindSolutionRoot(currentDirectory);
            if (solutionRoot != null && File.Exists(Path.Combine(solutionRoot, "appsettings.json")))
            {
                configBuilder.SetBasePath(solutionRoot);
                Console.WriteLine($"Found appsettings.json in solution root: {solutionRoot}");
            }
            else
            {
                Console.WriteLine("Could not find appsettings.json, using fallback configuration");
            }
        }
        
        var configuration = configBuilder
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection string from config: {connectionString}");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // 使用后备连接字符串
            connectionString = "server=localhost;port=3306;database=blog_ef_test;user=root;password=66666666;";
            Console.WriteLine($"Using fallback connection string: {connectionString}");
        }

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new BlogDbContext(optionsBuilder.Options);
    }
    
    private static string? FindSolutionRoot(string startPath)
    {
        var currentDir = new DirectoryInfo(startPath);
        while (currentDir != null)
        {
            // 查找解决方案文件或appsettings.json
            if (currentDir.GetFiles("*.sln").Length > 0 || 
                currentDir.GetFiles("appsettings.json").Length > 0)
            {
                return currentDir.FullName;
            }
            currentDir = currentDir.Parent;
        }
        return null;
    }
}