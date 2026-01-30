
using System.ComponentModel;
using MediatorXL.Abstractions;

namespace Example;

public class HelloMediatorRequest : IMessage
{
    public string Name { get; set; } = string.Empty;
}
