using Jim.Mddr.Example.Command;
using Jim.Mddr.Interfaces;

namespace Jim.Mddr.Example.Publishers;

public class Example1Publisher : IPublisher<TestRequest>
{
    public Task PublishAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Example1 Publisher: Event published.");
        return Task.CompletedTask;
    }
}
    