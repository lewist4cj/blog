using System.Reflection;
using Blog.Common;
using Blog.Core.Repository;
using Blog.Domain.Config;
using Blog.Extensions.Validation;

namespace Blog.Api.Infrastructure;

public static class CollectServiceExtension
{
    public static IServiceCollection AddRepositoryRegister(this IServiceCollection services)
    {
        // 直接注册泛型仓储，避免反射在单文件发布下失效
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }

    public static IServiceCollection AddServiceRegister(this IServiceCollection services, IConfiguration configuration)
    {
        var namespaceConfig = configuration.GetSection("IocTags").Get<IocTagsConfig>();
        namespaceConfig!.Validate();
        var list = namespaceConfig!.List!;
        list.ForEach(item =>
        {
            var ass = Assembly.Load(item);
            var implementTypes = ass.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(ITag))
                            && t is { IsAbstract: false, IsInterface: false });
            foreach (var implementType in implementTypes)
            {
                var interfaceType = implementType
                    .GetInterfaces()
                    .FirstOrDefault(i => i != typeof(ITag))!;
                services.AddTransient(interfaceType, implementType);
            }
        });
        return services;
    }
}
