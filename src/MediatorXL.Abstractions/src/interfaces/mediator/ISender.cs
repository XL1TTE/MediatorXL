
namespace MediatorXL.Abstractions;

public interface ISender
{
    /// <summary>
    /// Sends message as a notification, meaning that all the IEventListeners will recive it.
    /// </summary>
    /// <typeparam name="TMessage">Message type.</typeparam>
    /// <param name="message">Message object.</param>
    /// <param name="ct">Cancellation token.</param>
    Task Notify<TMessage>(TMessage message, CancellationToken ct = default) where TMessage : IMessage;

    /// <summary>
    /// Sends a message as a request, meaning that only one IRequestHandler will proccess it.
    /// </summary>
    /// <typeparam name="TResponse">Expected response type.</typeparam>
    /// <param name="message">Message object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Response object.</returns>
    Task<TResponse?> Request<TResponse>(IMessage<TResponse> message, CancellationToken ct = default);
}
