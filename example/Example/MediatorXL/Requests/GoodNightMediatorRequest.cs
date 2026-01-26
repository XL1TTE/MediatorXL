
using MediatorXL.Abstractions;

namespace Example;

public class GoodNightMediatorRequest : IRequest<string>
{
    public string Name { get; set; } = string.Empty;
}
