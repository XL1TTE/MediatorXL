
using System.Collections.Concurrent;
using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Middleware;
using MediatorXL.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL;

public class MediatorConfig
{
    public IEnumerable<Assembly> AssembliesToScan { get; set; } = Array.Empty<Assembly>();
    internal IEnumerable<IGlobalMiddleware> GlobalMiddlewares { get; set; } = Array.Empty<IGlobalMiddleware>();
}

public sealed class MediatorXL(
    IServiceProvider _serviceProvider,
    IEnumerable<IGlobalMiddleware> _globalMiddlewares) : IMediator
{
    private static ConcurrentDictionary<Type, object> _resolversCache = new();

    public async Task Notify<TMessage>(TMessage message, CancellationToken ct = default) where TMessage : IMessage
    {
        var handlers = _serviceProvider.GetServices<IEventListener<TMessage>>();

        await _globalMiddlewares.CallAllPreHandleMiddlewares(message, ct);

        foreach (var handler in handlers)
        {
            if (ct.IsCancellationRequested) { throw new OperationCanceledException(); }
            await handler.Handle(message, ct);
        }

        await _globalMiddlewares.CallAllPostHandleMiddlewares(message, ct);

    }
    public async Task<TResponse> Request<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {
        var requestType = message.GetType();

        var resolver = _resolversCache.GetOrAdd(requestType, (reqType) =>
        {
            var resolverType = typeof(HandlerResolver<,>).MakeGenericType(message.GetType(), typeof(TResponse));
            return Activator.CreateInstance(resolverType)!;
        });

        await _globalMiddlewares.CallAllPreHandleMiddlewares(message, ct);

        var response = await ((HandlerResolver<TResponse>)resolver).Resolve(message, _serviceProvider, ct);

        await _globalMiddlewares.CallAllPostHandleMiddlewares(message, response, ct);

        return response;
    }

}
