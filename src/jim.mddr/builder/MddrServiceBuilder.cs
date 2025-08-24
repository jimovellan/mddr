using System.Reflection;
using Jim.Mddr.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Jim.Mddr.Builder
{
    public class MddrServiceBuilder : IMddrServiceBuilder
    {
        private readonly IServiceCollection _services;

        public MddrServiceBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IMddrServiceBuilder RegisterMddrAndHandlers(List<Assembly> assemblies)
        {
            _services.AddScoped<ISender, MddrSender>();

            _services.Scan(a => a
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return this;
        }

        public IMddrServiceBuilder AddPublisher<TPublisher, TEntity>()
            where TEntity : class
            where TPublisher : IPublisher<TEntity>
        {
            _services.AddScoped(typeof(IPublisher<TEntity>), typeof(TPublisher));
            return this;
        }

        public IMddrServiceBuilder AddPipeline<TPipeline>()
            where TPipeline : IPipeline
        {

            _services.AddTransient(typeof(IPipeline), typeof(TPipeline));
            return this;
            
        }

        public IMddrServiceBuilder AddPublishersFromAssembly(Assembly assembly)
        {
            _services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IPublisher<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            return this;
        }

        public IMddrServiceBuilder AddPipelinesFromAssembly(Assembly assembly)
        {
            _services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<IPipeline>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            return this;
        }

        
    }
}