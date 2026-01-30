namespace MediatorXL.Abstractions;

/// <summary>
/// Middleware interface to define some logic which will be executed
/// when every single message is sended.
/// </summary>
/// <typeparam name="TMessage">Message type.</typeparam>
public interface IGlobalMiddleware
{
    /// <summary>
    /// Triggered before any handler.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task PreHandle(IMessage message, CancellationToken ct = default);

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
    Task PostHandle(IMessage message, CancellationToken ct = default);

    /// <summary>
    /// Triggered only when all the handlers completed their work successfully.
    /// </summary>
    Task PostHandle<TResponse>(IMessage<TResponse> message, TResponse response, CancellationToken ct = default);
}

/// <summary>
/// Middleware interface to define some logic which will be executed
/// when specific message is sended.
/// </summary>
/// <typeparam name="TMessage">Message type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IMessageMiddleware<in TMessage, TResponse>
where TMessage : IMessage<TResponse>
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
    Task PreHandle(IMessage<TResponse> message, CancellationToken ct = default);

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
    Task PostHandle(IMessage<TResponse> message, TResponse response, CancellationToken ct = default);
}


