
using MediatorXL.Abstractions;

namespace Example;

public class GoodByMediatorHandler : IRequestHandler<GoodNightMediatorRequest, string>
{
    public Task<string> Handle(GoodNightMediatorRequest request, CancellationToken ct = default)
    {
        Console.WriteLine($"GoodBy, {request.Name}!");
        return Task.FromResult("Completed");
    }
}
