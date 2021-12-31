using Basyc.MessageBus.Shared;
using Basyc.Shared.Helpers;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared;

public class TypedMessageToByteSerializer : IMessageToByteSerializer
{
    public byte[] Serialize(object? messageData, string messageType, int sessionId, MessageCase messageCase)
    {
        Type messageClrType = TypedToSimpleConverter.ConvertSimpleToType(messageType);
        var messageDataBytes = TypedObjectToByteSerializer.Serialize(messageData, messageClrType);
        var messageWrapper = new ProtoMessageWrapper(sessionId, messageCase, messageType, messageDataBytes);
        var wrapperBytes = TypedObjectToByteSerializer.Serialize(messageWrapper,typeof(ProtoMessageWrapper));
        return wrapperBytes;
    }


    public OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> Deserialize(byte[] commandBytes)
    {
        ProtoMessageWrapper messageWrapper = TypedObjectToByteSerializer.Deserialize<ProtoMessageWrapper>(commandBytes);
        Type messageClrType = TypedToSimpleConverter.ConvertSimpleToType(messageWrapper.MessageType);
        object messageData;
        try
        {
            messageData = TypedObjectToByteSerializer.Deserialize(messageWrapper.MessageData, messageClrType);
        }
        catch (Exception ex)
        {
            DeserializationFailureCase failure = new(messageWrapper.SessionId,messageWrapper.MessageCase, messageWrapper.MessageType, ex, $"{ex.Message}");
            return failure;
            //return DeserializationResult.CreateDeserializationFailure(failure);
        }



        switch (messageWrapper.MessageCase)
        {
            case MessageCase.CheckIn:
                var checkIn = (CheckInMessage)messageData;
                //return DeserializationResult.CreateCheckIn(checkIn);
                return checkIn;
            case MessageCase.Request:
                if (messageData is IMessage)
                {
                    RequestCase requestCase = new RequestCase(messageWrapper.SessionId, TypedToSimpleConverter.ConvertTypeToSimple(messageClrType), messageData, false, null);
                    //return DeserializationResult.CreateRequest(requestCase);
                    return requestCase;
                }
                if (GenericsHelper.IsAssignableToGenericType(messageClrType, typeof(IMessage<>)))
                {
                    var responseType = GenericsHelper.GetTypeArgumentsFromParent(messageClrType, typeof(IMessage<>))[0];
                    RequestCase requestCase = new RequestCase(messageWrapper.SessionId, TypedToSimpleConverter.ConvertTypeToSimple(messageClrType), messageData, true, responseType);
                    //return DeserializationResult.CreateRequest(requestCase);
                    return requestCase;
                }
                throw new Exception();
            case MessageCase.Response:
                ResponseCase responseCase = new ResponseCase(messageWrapper.SessionId, messageData, messageData.GetType());
                //return DeserializationResult.CreateResponse(responseCase);
                return responseCase;
            case MessageCase.Event:
                var eventCase = new EventCase(messageWrapper.SessionId, TypedToSimpleConverter.ConvertTypeToSimple(messageClrType), messageData);
                //return DeserializationResult.CreateEvent(eventCase);
                return eventCase;
        }

        throw new Exception();
    }
}
