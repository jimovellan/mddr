using Jim.Mddr.Interfaces;

namespace Jim.Mddr.Example.Command;

public class TestRequest : IRequest<int>
{
}


public class TestRequestHandler: IRequestHandler<TestRequest, int>
{
    public Task<int> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Handling TestRequest");
        return Task.FromResult(42);
    }
}
