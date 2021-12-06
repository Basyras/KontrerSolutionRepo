using Basyc.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public static class MessageSerializer
    {
        public static byte[] SerializeCommand<TMessage>(TMessage message, int sessionId) where TMessage : notnull
        {
            var serializedRequest = ProtoBufMessageSerializer.Serialize(message);
            var commandWrapper = new ProtoBufCommandWrapper(message.GetType().AssemblyQualifiedName!, serializedRequest, sessionId);
            var serializedWrapper = ProtoBufMessageSerializer.Serialize(commandWrapper);
            return serializedWrapper;
        }

        public static byte[] SerializeCommand<TMessage>(int sessionId)
        {
            var commandWrapper = new ProtoBufCommandWrapper(typeof(TMessage).AssemblyQualifiedName!, new byte[0], sessionId);
            var serializedWrapper = ProtoBufMessageSerializer.Serialize(commandWrapper);
            return serializedWrapper;
        }


        public static DeserializedMessageResult DeserializeMessage(byte[] commandBytes)
        {
            ProtoBufCommandWrapper messageWrapper = ProtoBufMessageSerializer.Deserialize<ProtoBufCommandWrapper>(commandBytes);

            Type messageType = Type.GetType(messageWrapper.CommandAssemblyQualifiedName!)!;
            object message = ProtoBufMessageSerializer.Deserialize(messageWrapper.CommandBytes, messageType);
            bool expectsResponse;
            if(message is IMessage)
            {
                expectsResponse = false;
            }
            else
            {
                if (GenericsHelper.IsAssignableToGenericType(messageType, typeof(IMessage<>)))
                {
                    expectsResponse = true;
                }
                else
                {
                    throw new Exception("message type not recognized");
                }
            }
            return new DeserializedMessageResult(messageWrapper.SessionId, expectsResponse, message, messageType);
        }
    }
}
