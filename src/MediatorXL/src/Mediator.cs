
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using MediatorXL.Abstractions;
using MediatorXL.Attributes;
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

        var middlewares = EventsMiddlewareCache<TMessage>.GetOrCreate(() =>
        {
            return _serviceProvider.GetServices<IEventMiddleware<TMessage>>()
            .OrderBy(m => m.GetType().GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0);
        });

        #region Pre-handlers global middleware
        foreach (var middleware in _globalMiddlewares)
        {
            var command = await middleware.BeforeEventHandle(message, ct);
            if (command is BreakRequestResult)
            {
                return;
            }
            else if (command is RetryRequestResult _retryCommand)
            {
                // Execute retry policy.
            }
            else if (command is ContinueRequestResult)
            {
                continue;
            }
        }
        #endregion

        #region Pre-handlers event middleware

        foreach (var middleware in middlewares)
        {
            var command = await middleware.BeforeEventHandle(message, ct);
            if (command is BreakRequestResult)
            {
                return;
            }
            else if (command is RetryRequestResult _retryCommand)
            {
                // Execute retry policy.
            }
            else if (command is ContinueRequestResult)
            {
                continue;
            }
        }

        #endregion

        #region Handlers execution
        foreach (var handler in handlers)
        {
            try { await handler.Handle(message, ct); }
            catch { break; }
        }
        #endregion

        #region Post-handlers global middleware
        foreach (var middleware in _globalMiddlewares)
        {
            await middleware.AfterEventHandle(message, ct);
        }
        #endregion

        #region Post-handlers event middleware
        foreach (var middleware in middlewares)
        {
            await middleware.AfterEventHandle(message, ct);
        }
        #endregion

    }
    public async Task<TResponse?> Request<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {
        var requestType = message.GetType();

        var resolver = _resolversCache.GetOrAdd(requestType, (reqType) =>
        {
            var resolverType = typeof(HandlerResolver<,>).MakeGenericType(message.GetType(), typeof(TResponse));
            return Activator.CreateInstance(resolverType)!;
        });

        var response = await ((HandlerResolver<TResponse>)resolver).Resolve(message, _globalMiddlewares, _serviceProvider, ct);


        return response;
    }

}
