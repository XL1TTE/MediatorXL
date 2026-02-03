
using MediatorXL.Abstractions;

namespace Example;

public class MediatorRequest : IMessage<string>
{
    public string InitialResponse = "Initial response message.";
}
