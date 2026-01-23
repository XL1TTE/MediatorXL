
using MediatorXL.Interfaces;

namespace Example;

public class HelloMediatorHandler(IServiceProvider serviceProvider) : IHandler<HelloMediatorRequest>
{
    public async Task Handle(HelloMediatorRequest request, CancellationToken ct = default)
    {
        Console.WriteLine($"Hello, MediatorXL! My name is {request.Name}! \n I even have: {serviceProvider}");
    }
}
