namespace Jim.Mddr.Interfaces;

public interface IPublisher<TEntity> where TEntity : class
{
    Task PublishAsync(TEntity entity, CancellationToken cancellationToken = default);
}
