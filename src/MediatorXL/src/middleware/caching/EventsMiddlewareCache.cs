using MediatorXL.Abstractions;

namespace MediatorXL;

internal static class EventsMiddlewareCache<TMessage> where TMessage : IMessage
{
    public static IEnumerable<IEventMiddleware<TMessage>> GetOrCreate(Func<IEnumerable<IEventMiddleware<TMessage>>> fabric)
    {
        if (m_Cache == null)
        {
            m_Cache = fabric().ToList();
        }
        return m_Cache;
    }

    private static IEnumerable<IEventMiddleware<TMessage>>? m_Cache = null;
}
