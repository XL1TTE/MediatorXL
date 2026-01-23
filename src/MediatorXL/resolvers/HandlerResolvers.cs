using MediatorXL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL.Resolvers;

internal abstract class HandlerResolver<TResponse>
{
    public abstract Task<TResponse> Resolve(
        IRequest<TResponse> request,
        IServiceProvider provider,
        CancellationToken ct);
}

internal class HandlerResolver<TRequest, TResponse> : HandlerResolver<TResponse>
    where TRequest : IRequest<TResponse>
{
    public override Task<TResponse> Resolve(IRequest<TResponse> request, IServiceProvider provider, CancellationToken ct)
    {
        var handler = provider.GetRequiredService<IHandler<TRequest, TResponse>>();
        return handler.Handle((TRequest)request, ct);
    }
}
