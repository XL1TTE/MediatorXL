
using MediatorXL.Abstractions;

namespace Example;

public class HelloMediatorRequest : IRequest
{
    public string Name { get; set; } = string.Empty;
}
