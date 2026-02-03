
using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Attributes;
using MediatorXL.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL.Middleware;


internal static class MiddlewareTool
{

    /// <summary>
    /// Finds all the implementation types of IGlobalMiddleware.
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    internal static void RegisterGlobalMiddlewares(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var types = assemblies.SelectMany(
            x => MediatorReflection.FindSubclassesOf(x, typeof(IGlobalMiddleware)))
            .Where(x => x.GetCustomAttribute<DisabledAttribute>() == null);

        foreach (var t in types)
        {
            services.AddTransient(typeof(IGlobalMiddleware), t);
        }
    }

    internal static IEnumerable<IGlobalMiddleware> GetMiddlewaresSortedByPriority(IServiceProvider serviceProvider)
    {
        var middlewares = serviceProvider.GetServices<IGlobalMiddleware>() ?? Array.Empty<IGlobalMiddleware>();

        return middlewares.OrderBy(m => m.GetType().GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0);
    }


    internal static async Task CallAllPreHandleMiddlewares(this IEnumerable<IGlobalMiddleware> middlewares,
                                                                IMessage message,
                                                                CancellationToken ct = default)
    {
        foreach (var middleware in middlewares)
        {
            await middleware.BeforeEventHandle(message, ct);
        }
    }
    internal static async Task CallAllPostHandleMiddlewares(this IEnumerable<IGlobalMiddleware> middlewares,
                                                                IMessage message,
                                                                CancellationToken ct = default)
    {
        foreach (var middleware in middlewares)
        {
            await middleware.AfterEventHandle(message, ct);
        }
    }

    internal static async Task CallAllPreHandleMiddlewares<TResponse>(
        this IEnumerable<IGlobalMiddleware> middlewares,
        IMessage<TResponse> message,
        CancellationToken ct = default)
    {
        foreach (var middleware in middlewares)
        {
            await middleware.BeforeRequestHandle(message, ct);
        }
    }
    internal static async Task CallAllPostHandleMiddlewares<TResponse>(
        this IEnumerable<IGlobalMiddleware> middlewares,
        IMessage<TResponse> message,
        TResponse response,
        CancellationToken ct = default)
    {
        foreach (var middleware in middlewares)
        {
            await middleware.AfterRequestHandle(message, response, ct);
        }
    }

}
