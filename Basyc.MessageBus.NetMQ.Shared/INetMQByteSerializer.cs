using OneOf;

namespace Basyc.MessageBus.NetMQ.Shared
{
	public interface INetMQByteSerializer
	{
		OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> Deserialize(byte[] commandBytes);
		byte[] Serialize(object? message, string messageType, int sessionId, MessageCase messageCase);
	}
}