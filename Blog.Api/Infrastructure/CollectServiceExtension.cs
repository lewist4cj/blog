using Blog.Common;
using Blog.Common.RedisModule;
using Blog.Core.Repository;
using Blog.Services.ConfigMgrApp;
using Blog.Services.LogMgrApp;
using Blog.Services.UserApp;

namespace Blog.Api.Infrastructure;

public static class CollectServiceExtension
{
    public static IServiceCollection AddRepositoryRegister(this IServiceCollection services)
    {
        // 直接注册泛型仓储，避免反射在单文件发布下失效
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }

    /// <summary>
    /// 显式注册所有服务（替代旧的程序集扫描方式，兼容 AOT）
    /// </summary>
    public static IServiceCollection AddServiceRegister(this IServiceCollection services)
    {
        // Blog.Services 层
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ISiteConfigService, SiteConfigService>();

        // Blog.Common 层
        services.AddScoped<IRedisWorker, RedisWorker>();

        return services;
    }
}
