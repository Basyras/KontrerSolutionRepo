namespace Basyc.MessageBus.NetMQ.Shared
{
    public record DeserializationFailureCase(int SessionId, MessageCase MessageCase, string MessageType, Exception Expcetion, string ErrorMessage);
}