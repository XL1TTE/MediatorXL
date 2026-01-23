
using System.Collections.Concurrent;
using MediatorXL.Interfaces;
using MediatorXL.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL;

public sealed class MediatorXL(IServiceProvider _serviceProvider) : IMediator
{
    private static ConcurrentDictionary<Type, object> _resolversCache = new();

    public Task Send<TRequest>(TRequest request, CancellationToken ct = default) where TRequest : IRequest
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var handlers = _serviceProvider.GetServices<IHandler<TRequest>>();

        foreach (var handler in handlers)
        {
            if (ct.IsCancellationRequested) { throw new OperationCanceledException(); }
            handler.Handle(request, ct);
        }
        return Task.CompletedTask;
    }
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        var requestType = request.GetType();

        var resolver = _resolversCache.GetOrAdd(requestType, (reqType) =>
        {
            var resolverType = typeof(HandlerResolver<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            return Activator.CreateInstance(resolverType)!;
        });

        return ((HandlerResolver<TResponse>)resolver).Resolve(request, _serviceProvider, ct);

    }

}
