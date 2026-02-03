using MediatorXL.Abstractions;

namespace MediatorXL;

internal static class RequestsMiddlewareCache<TMessage, TResponse> where TMessage : IMessage<TResponse>
{
    public static IEnumerable<IRequestMiddleware<TMessage, TResponse>> GetOrCreate(Func<IEnumerable<IRequestMiddleware<TMessage, TResponse>>> fabric)
    {
        if (m_Cache == null)
        {
            m_Cache = fabric().ToList();
        }
        return m_Cache;
    }

    private static IEnumerable<IRequestMiddleware<TMessage, TResponse>>? m_Cache = null;
}
