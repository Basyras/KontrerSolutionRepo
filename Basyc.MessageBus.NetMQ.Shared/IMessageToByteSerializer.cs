using OneOf;

namespace Basyc.MessageBus.NetMQ.Shared
{
    public interface IMessageToByteSerializer
    {
        //DeserializationResult Deserialize(byte[] commandBytes);
        OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> Deserialize(byte[] commandBytes);
        byte[] Serialize(object? message, string messageType, int sessionId, MessageCase messageCase);
    }
}