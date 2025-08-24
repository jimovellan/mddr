using Jim.Mddr.Interfaces;

namespace Jim.Mddr.Example.Pipelines
{
    public class Pipeline1 : IPipeline
    {
        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {

            Console.WriteLine("Pipeline1 before");
            return next(cancellationToken);
        }
    }
}