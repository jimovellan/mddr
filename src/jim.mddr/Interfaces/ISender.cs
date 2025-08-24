namespace Jim.Mddr.Interfaces;

public interface ISender
{
    //como se puede inferir el <TResponse> de TRequest?
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> command, CancellationToken cancellationToken = default);
}
