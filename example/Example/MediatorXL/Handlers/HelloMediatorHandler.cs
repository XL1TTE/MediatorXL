
using MediatorXL.Abstractions;

namespace Example;

public class HelloMediatorHandler(IServiceProvider serviceProvider) : IEventListener<HelloMediatorRequest>
{
    public async Task Handle(HelloMediatorRequest request, CancellationToken ct = default)
    {
        Console.WriteLine($"Hello, MediatorXL! My name is {request.Name}! \n I even have: {serviceProvider}");
    }
}

