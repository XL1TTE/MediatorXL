
using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Attributes;

namespace Example;

[Priority(0)]
public class GlobalTestMiddleware : IGlobalMiddleware
{
    public async Task PostHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine("1 GLOBAL POST MIDDLE!");
    }

    public async Task PostHandle<TResponse>(IMessage<TResponse> message, TResponse response, CancellationToken ct = default)
    {
        Console.WriteLine("1 GLOBAL POST MIDDLE FOR MESSAGE WITH RESPONSE!");
    }

    public async Task PreHandle(IMessage request, CancellationToken ct = default)
    {
        Console.WriteLine("1 GLOBAL PRE MIDDLE!");
    }

    public async Task PreHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {

        Console.WriteLine("1 GLOBAL PRE MIDDLE FOR MESSAGE WITH RESPONSE!");
        Console.WriteLine($"1 ATTRIBUTE: {message.GetType().GetCustomAttribute<ObsoleteAttribute>()?.GetType().Name}!");

    }
}

[Priority(1)]
public class Global2TestMiddleware : IGlobalMiddleware
{
    public async Task PostHandle(IMessage message, CancellationToken ct = default)
    {
        Console.WriteLine("2 GLOBAL POST MIDDLE!");
    }

    public async Task PostHandle<TResponse>(IMessage<TResponse> message, TResponse response, CancellationToken ct = default)
    {
        Console.WriteLine("2 GLOBAL POST MIDDLE FOR MESSAGE WITH RESPONSE!");
    }

    public async Task PreHandle(IMessage request, CancellationToken ct = default)
    {
        Console.WriteLine("2 GLOBAL PRE MIDDLE!");
    }

    public async Task PreHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {
        Console.WriteLine("2 GLOBAL PRE MIDDLE FOR MESSAGE WITH RESPONSE!");
        Console.WriteLine($"2 ATTRIBUTE: {message.GetType().GetCustomAttribute<ObsoleteAttribute>()?.GetType().Name}!");
    }
}
