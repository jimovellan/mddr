using System.Data;
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

    public void GetPipelines()
    {
        // Implementation for obtaining pipelines
        serviceProvider.GetServices<IPipeline>();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> command, CancellationToken cancellationToken = default)
    {

        var inputType = command.GetType();

        var outputType = typeof(TResponse);

        var pipelines = serviceProvider.GetServices<IPipeline>();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(inputType, outputType);

        var handler = serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for {command.GetType().Name}");
        }

        var method = handlerType.GetMethod("HandleAsync");

        Func<CancellationToken, Task<TResponse>> fnDelegate = async (CancellationToken ct = default) =>
        {
            var task = (Task<TResponse>)method.Invoke(handler, new object[] { command, ct });
            return await task;
        };



        Func<CancellationToken, Task<TResponse>> firstCall = null;

        List<Func<CancellationToken, Task<TResponse>>> calls = new List<Func<CancellationToken, Task<TResponse>>>();

        calls.Add(fnDelegate);
        //tengo que llamar de un pipeline a otro  en el primero a fnDelegate y en los demas al siguiente pipeline



        foreach (var pipeline in pipelines.Reverse())
        {
            var lastCall = calls.Last();
            Func<CancellationToken, Task<TResponse>> nextCall = async (CancellationToken ct = default) => await pipeline.SendAsync(command, lastCall, ct);
            calls.Add(nextCall);
        }

        return await calls.Last()(cancellationToken);
    }
}