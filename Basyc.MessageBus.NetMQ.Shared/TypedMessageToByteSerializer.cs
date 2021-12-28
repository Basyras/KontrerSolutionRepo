using Basyc.MessageBus.Shared;
using Basyc.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared;

public static class TypedMessageToByteSerializer
{
    public static byte[] Serialize<TMessage>(TMessage? message, int sessionId, bool isResponse = false)
    {
        return Serialize(message, typeof(TMessage), sessionId, isResponse);
    }

    public static byte[] Serialize(object message, int sessionId, bool isResponse = false)
    {
        return Serialize(message, message.GetType(), sessionId, isResponse);
    }

    public static byte[] Serialize(object? message, Type messageType, int sessionId, bool isResponse = false)
    {
        var serializedRequest = TypedObjectToByteSerializer.Serialize(message);
        var commandWrapper = new ProtoMessageWrapper(messageType.AssemblyQualifiedName!, serializedRequest, sessionId, isResponse);
        var serializedWrapper = TypedObjectToByteSerializer.Serialize(commandWrapper);
        return serializedWrapper;
    }

    public static byte[] Serialize<TMessage>(int sessionId, bool isResponse)
    {
        var commandWrapper = new ProtoMessageWrapper(typeof(TMessage).AssemblyQualifiedName!, new byte[0], sessionId, isResponse);
        var serializedWrapper = TypedObjectToByteSerializer.Serialize(commandWrapper);
        return serializedWrapper;
    }


    public static DeserializedMessage Deserialize(byte[] commandBytes)
    {
        ProtoMessageWrapper messageWrapper = TypedObjectToByteSerializer.Deserialize<ProtoMessageWrapper>(commandBytes);

        Type messageType = Type.GetType(messageWrapper.CommandAssemblyQualifiedName!)!;
        object messageData = TypedObjectToByteSerializer.Deserialize(messageWrapper.CommandBytes, messageType);

        if (messageData is CheckInMessage checkIn)
        {
            return DeserializedMessage.CreateCheckIn(checkIn);
        }

        if (messageData is IMessage)
        {
            return DeserializedMessage.CreateRequest(new RequestCase(messageWrapper.SessionId, messageType.FullName!, messageData, false, null));
        }

        if (GenericsHelper.IsAssignableToGenericType(messageType, typeof(IMessage<>)))
        {
            var responseType = GenericsHelper.GetTypeArgumentsFromParent(messageType, typeof(IMessage<>))[0];
            return DeserializedMessage.CreateRequest(new RequestCase(messageWrapper.SessionId, TypedToSimpleConverter.ConvertTypeToSimple(messageType), messageData, true, responseType));
        }

        return DeserializedMessage.CreateResponse(new ResponseCase(messageWrapper.SessionId, messageData, messageData.GetType()));


    }
}
