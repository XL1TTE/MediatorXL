
using System.Collections.Concurrent;
using System.ComponentModel;
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

        #region Pre-handlers middleware
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

        #region Handlers execution
        foreach (var handler in handlers)
        {
            try { await handler.Handle(message, ct); }
            catch { break; }
        }
        #endregion

        #region Post-handlers middleware
        foreach (var middleware in _globalMiddlewares)
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

        #region Pre-handlers middleware
        foreach (var middleware in _globalMiddlewares)
        {
            var command = await middleware.BeforeRequestHandle(message, ct);
            if (command is BreakRequestResult)
            {
                if (command is BreakRequestResult<TResponse> _breakCommand) { return _breakCommand.Response; }
                else { return default; }
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

        var response = await ((HandlerResolver<TResponse>)resolver).Resolve(message, _serviceProvider, ct);

        #region Post-handlers middleware
        foreach (var middleware in _globalMiddlewares)
        {
            await middleware.AfterRequestHandle(message, response, ct);
        }
        #endregion

        return response;
    }

}
