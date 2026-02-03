namespace MediatorXL.Abstractions;


public abstract class IBaseMiddleware
{
    /// <summary>
    /// Tells pipeline to move to the next step.
    /// </summary>
    protected IMiddlewareResult Continue() => new ContinueRequestResult();
    /// <summary>
    /// Tells pipeline to stop processing the message.
    /// </summary>
    protected IMiddlewareResult Break() => new BreakRequestResult();
    /// <summary>
    /// Tells pipeline to stop processing the message.
    /// </summary>
    /// <param name="response">Response that will be returned if message awaits for it.</param>
    protected IMiddlewareResult Break<TResponse>(TResponse response) => new BreakRequestResult<TResponse>(response);
    /// <summary>
    /// Tells pipeline to queue message for delayed execution. 
    /// </summary>
    /// <param name="delay">Delay of execution in milliseconds.</param>
    protected IMiddlewareResult Retry(int delay) => new RetryRequestResult(delay);
}

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

/// <summary>
/// Middleware interface to define some logic which will be executed
/// when specific message is sended.
/// </summary>
/// <typeparam name="TMessage">Message type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IMessageMiddleware<in TMessage>
where TMessage : IMessage
{
    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task PreHandle(TMessage message, CancellationToken ct = default);

    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    Task PreHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default);

    /// <summary>
    /// Triggered only when all the handlers completed their work successfully.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="response"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task PostHandle(TMessage message, CancellationToken ct = default);

    /// <summary>
    /// Triggered only when all the handlers completed their work successfully.
    /// </summary>
    Task PostHandle<TResponse>(IMessage<TResponse> message, TResponse response, CancellationToken ct = default);
}


