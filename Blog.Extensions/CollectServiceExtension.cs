using System.Reflection;
using Blog.Common;
using Blog.Common.Utils;
using Blog.Extensions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blog.Extensions;

public static class CollectServiceExtension
{
    public static IServiceCollection AddRepositoryRegister(this IServiceCollection servicesCollection)
    {
        var assCore = Assembly.Load("Blog.Core");
        var implementType = assCore.GetTypes().FirstOrDefault(item => item.Name == "Repository`1")!;
        var interfaceType = implementType.GetInterface("IRepository`1")!.GetGenericTypeDefinition();
        servicesCollection.AddTransient(interfaceType, implementType); 
        return servicesCollection;
    }

    public static IServiceCollection AddServiceRegister(this IServiceCollection servicesCollection)
    {
        // 获取日志实例，记录加载完成的日志
        Log.Information("start service register ...");
        
        var namspaceName = AppSettings.Configuration?.GetSection("IocTags").Get<IocTagsConfig>();
        var list = namspaceName?.ValidateAndReturn().List!;
        list.ForEach(item =>
        {
            var ass = Assembly.Load(item);
            var implementTypes = ass.GetTypes()
                .Where(item => item.IsAssignableTo(typeof(ITag))
                               && item is { IsAbstract: false, IsInterface: false });
            foreach (var implementType in implementTypes)
            {
                var interfaceType = implementType
                    .GetInterfaces()
                    .FirstOrDefault(item => item != typeof(ITag))!;
                     servicesCollection.AddTransient(interfaceType, implementType);

            }
        }
        );
        Log.Information("service register complete, total: ",list.Count);

        return servicesCollection;
    }
}