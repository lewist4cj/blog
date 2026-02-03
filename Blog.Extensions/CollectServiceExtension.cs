using System.Reflection;
using Blog.Domain;
using Microsoft.Extensions.DependencyInjection;
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
        var assService = Assembly.Load("Blog.Services");
        var implementTypes = assService.GetTypes()
            .Where(item => item.IsAssignableTo(typeof(ITag))
                           && item is { IsAbstract: false, IsInterface: false });
        foreach (var implementType in implementTypes)
        {
            var interfaceType = implementType
                .GetInterfaces()
                .FirstOrDefault(item => item != typeof(ITag))!;

            servicesCollection.AddTransient(interfaceType, implementType);
        }
        return servicesCollection;
    }
}