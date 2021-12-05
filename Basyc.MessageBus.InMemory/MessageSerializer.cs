using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.InMemory
{
    public static class MessageSerializer
    {
        public static byte[] SerializeCommand<TMessage>(TMessage message, int sessionId)
        {
            var serializedRequest = BinarySerializer.Serialize(message);
            var commandWrapper = new ProtoBufCommandWrapper(message.GetType().AssemblyQualifiedName!, serializedRequest, sessionId);
            var serializedWrapper = BinarySerializer.Serialize(commandWrapper);
            return serializedWrapper;
        }

        public static byte[] SerializeCommand<TMessage>(int sessionId)
        {
            var commandWrapper = new ProtoBufCommandWrapper(typeof(TMessage).AssemblyQualifiedName!, new byte[0], sessionId);
            var serializedWrapper = BinarySerializer.Serialize(commandWrapper);
            return serializedWrapper;
        }


        public static (int sessionId, object? message) DeserializeMessage(byte[] commandBytes)
        {
            ProtoBufCommandWrapper commandWrapper = BinarySerializer.Deserialize<ProtoBufCommandWrapper>(commandBytes);

            Type responseType = Type.GetType(commandWrapper.CommandAssemblyQualifiedName!)!;

            object? response = BinarySerializer.Deserialize(commandWrapper.CommandBytes, responseType);

            return (commandWrapper.CommunicationId, response);
        }
    }
}
