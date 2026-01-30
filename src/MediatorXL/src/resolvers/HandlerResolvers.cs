using MediatorXL.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL.Resolvers;

internal abstract class HandlerResolver<TResponse>
{
    public abstract Task<TResponse> Resolve(
        IMessage<TResponse> request,
        IServiceProvider provider,
        CancellationToken ct);
}

internal class HandlerResolver<TRequest, TResponse> : HandlerResolver<TResponse>
    where TRequest : IMessage<TResponse>
{
    public override Task<TResponse> Resolve(IMessage<TResponse> request, IServiceProvider provider, CancellationToken ct)
    {
        var handler = provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return handler.Handle((TRequest)request, ct);
    }
}
