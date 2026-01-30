
namespace MediatorXL.Abstractions;

public interface IMessage { }

public interface IMessage<out TResponse> : IMessage
{ }
