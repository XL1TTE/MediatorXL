namespace MediatorXL.Interfaces;

public interface IHandler<in TRequest> where TRequest : IRequest
{
    Task Handle(TRequest request, CancellationToken ct = default);
}

public interface IHandler<in TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken ct = default);
}
