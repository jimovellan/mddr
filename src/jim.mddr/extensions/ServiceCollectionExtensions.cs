using System.Reflection;
using Jim.Mddr.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Jim.Mddr.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Mddr services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMddr(this IServiceCollection services, List<Assembly> assemblies)
    {
        services.AddScoped<ISender, MddrSender>();

        services.Scan(a => a.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }


    /// <summary>
    /// Adds the Mddr services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddMddr(this IServiceCollection services, Assembly assembly)
    {
        return services.AddMddr(new List<Assembly> { assembly });
    }

    /// <summary>
    /// Adds the Mddr services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>

    public static IServiceCollection AddMddr(this IServiceCollection services, List<string> assemblyNames)
    {
        var assemblies = assemblyNames.Select(Assembly.Load).ToList();
        return services.AddMddr(assemblies);
    }
    
    

}
