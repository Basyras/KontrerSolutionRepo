using OneOf;

namespace Basyc.MessageBus.NetMQ.Shared
{
	public interface INetMQMessageWrapper
	{
		OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> ReadWrapperMessage(byte[] messageBytes);
		byte[] CreateWrapperMessage(object? message, string messageType, int sessionId, MessageCase messageCase);
	}
}