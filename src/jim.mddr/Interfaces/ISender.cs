namespace Jim.Mddr.Interfaces;

public interface ISender
{
    /// <summary>
    /// Sends a command asynchronously.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> command, CancellationToken cancellationToken = default);


    /// <summary>
    /// Publishes an event asynchronously.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
}
