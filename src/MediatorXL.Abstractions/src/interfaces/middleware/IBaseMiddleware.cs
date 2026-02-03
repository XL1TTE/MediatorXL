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


