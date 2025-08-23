using Jim.Mddr.Interfaces;

namespace Jim.Mddr.Pipelines
{
    public class LoggingPipeline : IPipeline
    {
        private readonly ILogger<LoggingPipeline> logger;

        public LoggingPipeline(ILogger<LoggingPipeline> logger)
        {
            this.logger = logger;
        }

        

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken = default)
        {
            
            Console.WriteLine($"Pipeline Logging {typeof(TResponse).Name}");

            return next(cancellationToken);
        }
    }
}