
using MediatorXL.Abstractions;

namespace Example;

public class MediatorEvent : IMessage
{
    public string Data = "Data: {name: 'MediatorXL', messageType: 'event'}";
}
