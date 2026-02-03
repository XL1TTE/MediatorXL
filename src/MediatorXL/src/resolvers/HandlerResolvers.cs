using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL.Resolvers;

internal abstract class HandlerResolver<TResponse>
{
    public abstract Task<TResponse?> Resolve(
        IMessage<TResponse> request,
        IEnumerable<IGlobalMiddleware> globalMiddlewares,
        IServiceProvider provider,
        CancellationToken ct);
}

internal class HandlerResolver<TMessage, TResponse> : HandlerResolver<TResponse>
    where TMessage : IMessage<TResponse>
{
    public override async Task<TResponse?> Resolve(IMessage<TResponse> message, IEnumerable<IGlobalMiddleware> globalMiddlewares, IServiceProvider provider, CancellationToken ct)
    {
        var middlewares = RequestsMiddlewareCache<TMessage, TResponse>.GetOrCreate(() =>
        {
            return provider.GetServices<IRequestMiddleware<TMessage, TResponse>>()
            .OrderBy(m => m.GetType().GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0);
        });

        #region Pre-handlers global middleware
        foreach (var middleware in globalMiddlewares)
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

        #region Pre-handlers request middleware

        foreach (var middleware in middlewares)
        {
            var command = await middleware.BeforeRequestHandle((TMessage)message, ct);
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

        var handler = provider.GetRequiredService<IRequestHandler<TMessage, TResponse>>();

        var response = await handler.Handle((TMessage)message, ct);

        #region Post-handlers global middleware
        foreach (var middleware in globalMiddlewares)
        {
            await middleware.AfterRequestHandle(message, response, ct);
        }
        #endregion

        #region Post-handlers request middleware

        foreach (var middleware in middlewares)
        {
            await middleware.AfterRequestHandle((TMessage)message, response, ct);
        }

        #endregion

        return response;
    }
}
