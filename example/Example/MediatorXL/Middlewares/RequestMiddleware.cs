
using MediatorXL.Abstractions;
using MediatorXL.Attributes;

namespace Example;

[Priority(0)]
public class RequestMiddleware : BaseRequestMiddleware<MediatorRequest, string>
{
    public override async Task<IMiddlewareResult> BeforeRequestHandle(MediatorRequest message, CancellationToken ct = default)
    {
        Console.WriteLine($"[Priority - 0] [RequestMiddleware] [BEFORE] [Request: {message}].");
        return Continue();
    }

    public override async Task AfterRequestHandle(MediatorRequest message, string response, CancellationToken ct = default)
    {
        Console.WriteLine($"[Priority - 0] [EventMiddleware] [AFTER] [Request: {message}].");
    }
}
