using System.Reflection;
using Jim.Mddr.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Jim.Mddr.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMddr(this IServiceCollection services, List<Assembly> assemblies)
    {
        services.AddScoped<ISender, MddrSender>();


        var classes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            .ToList();

            foreach (var cls in classes)
            {
                var interfaces = cls.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .ToList();

                foreach (var iface in interfaces)
                {
                    services.AddTransient(iface, cls);
                }
            }

        return services;
    }

    public static IServiceCollection AddMddr(this IServiceCollection services, Assembly assembly)
    {
        return services.AddMddr(new List<Assembly> { assembly });
    }
    
    public static IServiceCollection AddMddr(this IServiceCollection services, List<string> assemblyNames)
    {

        
        var assemblies = assemblyNames.Select(Assembly.Load).ToList();
        return services.AddMddr(assemblies);
    }
    
}
