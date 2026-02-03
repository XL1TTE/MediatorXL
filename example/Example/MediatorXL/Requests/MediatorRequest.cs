
using MediatorXL.Abstractions;

namespace Example;

public class MediatorRequest : IMessage<string>
{
    public string Data = "Data: {name: 'MediatorXL', messageType: 'request'}";
}
