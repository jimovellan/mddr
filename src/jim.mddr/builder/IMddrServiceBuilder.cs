using System.Reflection;
using Jim.Mddr.Interfaces;

namespace Jim.Mddr.Builder
{
    public interface IMddrServiceBuilder
    {


        IMddrServiceBuilder AddPublisher<TPublisher, TEntity>()
            where TEntity : class
            where TPublisher : IPublisher<TEntity>;

        IMddrServiceBuilder AddPipeline<TPipeline>()
            where TPipeline : IPipeline;

        IMddrServiceBuilder AddPublishersFromAssembly(Assembly assembly);

        IMddrServiceBuilder AddPipelinesFromAssembly(Assembly assembly);
    }
}