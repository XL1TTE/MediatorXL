namespace MediatorXL.Abstractions;

public interface IRequestHandler<in TMessage, TResponse>
where TMessage : IMessage<TResponse>
{
    Task<TResponse> Handle(TMessage message, CancellationToken ct = default);
}


