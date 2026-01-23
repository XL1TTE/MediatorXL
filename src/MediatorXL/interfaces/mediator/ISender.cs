
namespace MediatorXL.Interfaces;

public interface ISender
{
    Task Send<TRequest>(TRequest request, CancellationToken ct = default) where TRequest : IRequest;
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}
