
namespace MediatorXL.Abstractions;

public interface IMiddlewareResult { }

public class BreakRequestResult : IMiddlewareResult { }
public sealed class BreakRequestResult<TResponse> : BreakRequestResult
{
    public BreakRequestResult(TResponse response)
    {
        this.Response = response;
    }
    public readonly TResponse Response;
}

public sealed class ContinueRequestResult : IMiddlewareResult { }

public sealed class RetryRequestResult : IMiddlewareResult
{

    /// <param name="delay">Delay in milliseconds.</param>
    public RetryRequestResult(int delay)
    {
        this.Delay = delay;
    }
    public readonly int Delay;
}
