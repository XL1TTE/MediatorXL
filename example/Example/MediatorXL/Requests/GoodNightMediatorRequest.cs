
using MediatorXL.Abstractions;

namespace Example;

[Obsolete]
public class GoodNightMediatorRequest : IMessage<string>
{
    public string Name { get; set; } = string.Empty;
}
