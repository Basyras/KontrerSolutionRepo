using OneOf;

namespace Basyc.MessageBus.NetMQ.Shared
{
	public interface INetMQByteMessageSerializer
	{
		OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> Deserialize(byte[] messageBytes);
		byte[] Serialize(object? message, string messageType, int sessionId, MessageCase messageCase);
	}
}