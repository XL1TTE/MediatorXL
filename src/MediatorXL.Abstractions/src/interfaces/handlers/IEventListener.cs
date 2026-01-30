namespace MediatorXL.Abstractions;

public interface IEventListener<in TMessage> where TMessage : IMessage
{
    Task Handle(TMessage message, CancellationToken ct = default);
}


