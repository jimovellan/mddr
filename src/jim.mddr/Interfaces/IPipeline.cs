namespace Jim.Mddr.Interfaces;

public interface IPipeline
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default);
}
