namespace MediatorXL.Abstractions;


public interface IEventMiddleware<TMessage> where TMessage : IMessage
{
    Task<IMiddlewareResult> BeforeEventHandle(TMessage message, CancellationToken ct = default);
    Task AfterEventHandle(TMessage message, CancellationToken ct = default);
}

/// <summary>
/// Middleware interface to define some logic which will be executed
/// when specific message is sended.
/// </summary>
/// <typeparam name="TMessage">Message type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public abstract class BaseEventMiddleware<TMessage> : IBaseMiddleware, IEventMiddleware<TMessage>
where TMessage : IMessage
{
    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task<IMiddlewareResult> BeforeEventHandle(TMessage message, CancellationToken ct = default)
    {
        return Continue();
    }

    /// <summary>
    /// Triggered only when all the handlers completed their work successfully.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="response"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task AfterEventHandle(TMessage message, CancellationToken ct = default)
    {
        return;
    }
}


