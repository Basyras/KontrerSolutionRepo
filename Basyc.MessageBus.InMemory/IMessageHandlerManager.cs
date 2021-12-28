
namespace Basyc.MessageBus.Client.NetMQ
{
    public interface IMessageHandlerManager
    {
        /// <summary>
        /// return response object or <see cref="Basyc.MessageBus.NetMQ.Shared.VoidResult"/>
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="messageData"></param>
        /// <returns></returns>
        Task<object> ConsumeMessage(string messageType, object messageData);
        string[] GetConsumableMessageTypes();
    }
}