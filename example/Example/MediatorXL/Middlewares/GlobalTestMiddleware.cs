
using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Attributes;

namespace Example;

[Priority(0)]
public class NotBreakingMiddleware : IGlobalMiddleware
{
    public override async Task<IMiddlewareResult> BeforeEventHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine($"This middleware have completed it's work before any event handler of message: {message.GetType().FullName}");
        return Continue();
    }
    public override async Task AfterEventHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine($"This middleware have completed it's work after all event handlers of message: {message.GetType().FullName}");
    }

    public override async Task<IMiddlewareResult> BeforeRequestHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {
        Console.WriteLine($"This middleware have completed it's work before any request handler of message: {message.GetType().FullName}");
        return Continue();
    }

    public override async Task AfterRequestHandle<TResponse>(IMessage message, TResponse response, CancellationToken ct = default)
    {
        Console.WriteLine($"This middleware have completed it's work after any request handler of message: {message.GetType().FullName}. Expected response: {response}");
    }
}

[Priority(0), Disabled]
public class BreakingMiddleware : IGlobalMiddleware
{
    public override async Task<IMiddlewareResult> BeforeEventHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine($"{nameof(BreakingMiddleware)}: Execution of breaking middleware for events.");
        return Break();
    }
    public override async Task AfterEventHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine("This command will not be executed because we breaked in pre-handle.");
    }

    public override async Task<IMiddlewareResult> BeforeRequestHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {
        Console.WriteLine($"{nameof(BreakingMiddleware)}: Execution of breaking middleware for requests.");
        return Break("We breaked from request handle pipeline. We only got this message, because type passed in Break function was the same as the TResponse type of message. Would get default of TResponse otherway.");
    }

    public override async Task AfterRequestHandle<TResponse>(IMessage message, TResponse response, CancellationToken ct = default)
    {
        Console.WriteLine("This command will not be executed because we breaked in pre-handle.");
    }
}



[Priority(1000)]
public class LogInConsoleMiddleware : IGlobalMiddleware
{
    public override async Task<IMiddlewareResult> BeforeEventHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine($"I'm the {nameof(LogInConsoleMiddleware)} and i have been executed before any event handler.");
        return Continue();
    }
    public override async Task AfterEventHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine($"I'm the {nameof(LogInConsoleMiddleware)} and i have been executed after all event handlers.");
    }
}
