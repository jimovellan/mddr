using Jim.Mddr.Example.Command;
using Jim.Mddr.Interfaces;

namespace Jim.Mddr.Example.Publishers;

public class Example2Publisher : IPublisher<TestRequest>
{
    public Task PublishAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Example2 Publisher: Event published.");
        return Task.CompletedTask;
    }
}
    