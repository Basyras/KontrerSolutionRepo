using Basyc.MessageBus.Shared;
using Basyc.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared;

public static class MessageSerializer
{
    public static byte[] Serialize<TMessage>(TMessage message, int sessionId, bool isResponse = false) where TMessage : notnull
    {
        var serializedRequest = ProtoBufMessageSerializer.Serialize(message);
        var commandWrapper = new ProtoMessageWrapper(message.GetType().AssemblyQualifiedName!, serializedRequest, sessionId, isResponse);
        var serializedWrapper = ProtoBufMessageSerializer.Serialize(commandWrapper);
        return serializedWrapper;
    }

    public static byte[] SerializeCommand<TMessage>(int sessionId, bool isResponse)
    {
        var commandWrapper = new ProtoMessageWrapper(typeof(TMessage).AssemblyQualifiedName!, new byte[0], sessionId, isResponse);
        var serializedWrapper = ProtoBufMessageSerializer.Serialize(commandWrapper);
        return serializedWrapper;
    }


    public static DeserializedMessageResult DeserializeMessage(byte[] commandBytes)
    {
        ProtoMessageWrapper messageWrapper = ProtoBufMessageSerializer.Deserialize<ProtoMessageWrapper>(commandBytes);

        Type messageType = Type.GetType(messageWrapper.CommandAssemblyQualifiedName!)!;
        object message = ProtoBufMessageSerializer.Deserialize(messageWrapper.CommandBytes, messageType);
        bool expectsResponse = false;
        Type? responseType = null;
        bool isResponse = false;
        if (message is IMessage)
        {
            expectsResponse = false;
        }
        else
        {
            if (GenericsHelper.IsAssignableToGenericType(messageType, typeof(IMessage<>)))
            {
                expectsResponse = true;
                responseType = GenericsHelper.GetTypeArgumentsFromParent(messageType, typeof(IMessage<>))[0];
            }
            else
            {
                isResponse = true;
                //throw new Exception("message type not recognized");

            }
        }
        return new DeserializedMessageResult(messageWrapper.SessionId, isResponse, expectsResponse, message, messageType, responseType);
    }
}
