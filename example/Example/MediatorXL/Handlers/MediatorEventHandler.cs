
using MediatorXL.Abstractions;

namespace Example;

public class MediatorEventHandler : IEventListener<MediatorEvent>
{
    public async Task Handle(MediatorEvent message, CancellationToken ct = default)
    {
        Console.WriteLine($"I'm {nameof(MediatorEventHandler)} and i've got event notification.");
    }
}
