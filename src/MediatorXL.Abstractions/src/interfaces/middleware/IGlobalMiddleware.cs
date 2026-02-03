namespace MediatorXL.Abstractions;

/// <summary>
/// Middleware interface to define some logic which will be executed
/// when every single message is sended.
/// </summary>
/// <typeparam name="TMessage">Message type.</typeparam>
public abstract class IGlobalMiddleware : IBaseMiddleware
{
    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task<IMiddlewareResult> BeforeEventHandle(IMessage message, CancellationToken ct = default)
    {
        return Continue();
    }
    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async Task<IMiddlewareResult> BeforeRequestHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
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
    public virtual async Task AfterEventHandle(IMessage message, CancellationToken ct = default)
    {
        return;
    }

    /// <summary>
    /// Triggered only when all the handlers completed their work successfully.
    /// </summary>
    public virtual async Task AfterRequestHandle<TResponse>(IMessage message, TResponse response, CancellationToken ct = default)
    {
        return;
    }
}


