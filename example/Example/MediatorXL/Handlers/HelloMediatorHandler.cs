
using MediatorXL.Abstractions;

namespace Example;

public class MediatorRequestHandler(IServiceProvider serviceProvider) : IRequestHandler<MediatorRequest, string>
{
    public async Task<string> Handle(MediatorRequest message, CancellationToken ct = default)
    {
        Console.WriteLine($"I'm the {nameof(MediatorRequestHandler)} and i've got a message!");
        return "Mediator request handler response.";
    }
}

