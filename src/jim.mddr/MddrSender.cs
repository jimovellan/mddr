using System.Data;
using Jim.Mddr.Extensions;
using Jim.Mddr.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Jim.Mddr;
public class MddrSender : ISender, IDisposable
{
    private readonly IServiceProvider serviceProvider;


    public MddrSender(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
        
    }

   
    /// <summary>
    /// Publishes an event to all registered subscribers.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task PublishAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)  where TEntity : class
    {
        var publishers = serviceProvider.GetServices(typeof(IPublisher<TEntity>)).ToArray();

        if (publishers.HasNoElements()) return;
        
        foreach (var publisher in publishers)
        {
            if(publisher is IPublisher<TEntity> typedPublisher)
            {
                await typedPublisher.PublishAsync(entity, cancellationToken);
                
            }else
            {
                throw new InvalidOperationException($"The publisher is not of the expected type {typeof(IPublisher<TEntity>).Name}");
            }
            
            
        }
    }

    /// <summary>
    /// Sends a command and returns the response through all pipelines configured.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> command, CancellationToken cancellationToken = default)
    {
        Func<CancellationToken, Task<TResponse>> fnDelegate = GetDelegateToCall(command);

        List<Func<CancellationToken, Task<TResponse>>> calls = new List<Func<CancellationToken, Task<TResponse>>>();

        calls.Add(fnDelegate);

        var pipelines = serviceProvider.GetServices<IPipeline>();

        foreach (var pipeline in pipelines.Reverse())
        {
            var lastCall = calls.Last();
            Func<CancellationToken, Task<TResponse>> nextCall = async (CancellationToken ct = default) => await pipeline.SendAsync(command, lastCall, ct);
            calls.Add(nextCall);
        }

        return await calls.Last()(cancellationToken);
    }

    private Func<CancellationToken, Task<TResponse>> GetDelegateToCall<TResponse>(IRequest<TResponse> command)
    {
        var inputType = command.GetType();

        var outputType = typeof(TResponse);

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(inputType, outputType);

        var handler = serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for {command.GetType().Name}");
        }

        var method = handlerType.GetMethod("HandleAsync");

        Func<CancellationToken, Task<TResponse>> fnDelegate = async (CancellationToken ct = default) =>
        {
            var task = method?.Invoke(handler, new object[] { command, ct }) as Task<TResponse>;

            if (task == null) throw new DataException("The task returned by the handler is null");

            return await task;
        };
        return fnDelegate;
    }
}