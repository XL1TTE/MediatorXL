namespace MediatorXL.Abstractions;

public interface IRequestMiddleware<TMessage, TResponse> where TMessage : IMessage<TResponse>
{
    Task<IMiddlewareResult> BeforeRequestHandle(TMessage message, CancellationToken ct = default);
    Task AfterRequestHandle(TMessage message, TResponse response, CancellationToken ct = default);
}

/// <summary>
/// Middleware interface to define some logic which will be executed
/// when specific message is sended.
/// </summary>
/// <typeparam name="TMessage">Message type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public abstract class BaseRequestMiddleware<TMessage, TResponse> : IBaseMiddleware, IRequestMiddleware<TMessage, TResponse>
where TMessage : IMessage<TResponse>
{
    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task<IMiddlewareResult> BeforeRequestHandle(TMessage message, CancellationToken ct = default)
    {
        return Continue();
    }

    /// <summary>
    /// Triggered only when all the handlers completed their work successfully.
    /// </summary>
    public virtual async Task AfterRequestHandle(TMessage message, TResponse response, CancellationToken ct = default)
    {
        return;
    }
}


