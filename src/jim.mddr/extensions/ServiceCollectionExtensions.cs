using System.Reflection;
using Jim.Mddr.Builder;
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
    public static IMddrServiceBuilder AddMddr(this IServiceCollection services, List<Assembly> assemblies)
    {
        return  new MddrServiceBuilder(services).RegisterMddrAndHandlers(assemblies);
       
    }


    /// <summary>
    /// Adds the Mddr services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IMddrServiceBuilder AddMddr(this IServiceCollection services, Assembly assembly)
    {
        return services.AddMddr(new List<Assembly> { assembly });
    }

    /// <summary>
    /// Adds the Mddr services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>

    public static IMddrServiceBuilder AddMddr(this IServiceCollection services, List<string> assemblyNames)
    {
        var assemblies = assemblyNames.Select(Assembly.Load).ToList();
        return services.AddMddr(assemblies);
    }
    
    

}
