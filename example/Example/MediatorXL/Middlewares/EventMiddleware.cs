
using MediatorXL.Abstractions;
using MediatorXL.Attributes;

namespace Example;


[Priority(0)]
public class EventMiddleware : BaseEventMiddleware<MediatorEvent>
{
    public override async Task<IMiddlewareResult> BeforeEventHandle(MediatorEvent message, CancellationToken ct = default)
    {
        Console.WriteLine($"[Priority - 0] [EventMiddleware] [BEFORE] [Event: {message}].");
        return Continue();
    }
    public override async Task AfterEventHandle(MediatorEvent message, CancellationToken ct = default)
    {
        Console.WriteLine($"[Priority - 0] [EventMiddleware] [AFTER] [Event: {message}].");
    }
}
[Priority(1000)]
public class EventMiddlewareHighPriority : BaseEventMiddleware<MediatorEvent>
{
    public override async Task<IMiddlewareResult> BeforeEventHandle(MediatorEvent message, CancellationToken ct = default)
    {
        Console.WriteLine($"[Priority - 1000] [EventMiddleware] [BEFORE] [Event: {message}].");
        return Continue();
    }
    public override async Task AfterEventHandle(MediatorEvent message, CancellationToken ct = default)
    {
        Console.WriteLine($"[Priority - 1000] [EventMiddleware] [AFTER] [Event: {message}].");
    }
}

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
