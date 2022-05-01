using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Basyc.Shared.Helpers;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared;

public class BasycTypedMessageToByteSerializer : IMessageToByteSerializer
{
    private readonly ISimpleByteSerailizer byteSerailizer;
    private readonly string wrapperMessageType;

    public BasycTypedMessageToByteSerializer(ISimpleByteSerailizer byteSerailizer)
    {
        this.byteSerailizer = byteSerailizer;
        wrapperMessageType = TypedToSimpleConverter.ConvertTypeToSimple<ProtoMessageWrapper>();

    }
    public byte[] Serialize(object messageData, string messageType, int sessionId, MessageCase messageCase)
    {
        var messageDataSeriliazationResult = byteSerailizer.Serialize(messageData, messageType);
        if (messageDataSeriliazationResult.IsT1)
            throw new Exception(messageDataSeriliazationResult.AsT1.Message);


        var wrapperMessageData = new ProtoMessageWrapper(sessionId, messageCase, messageType, messageDataSeriliazationResult.AsT0);
        var wrapperSerializationResult = byteSerailizer.Serialize(wrapperMessageData,wrapperMessageType );
        if (wrapperSerializationResult.IsT1)
            throw new Exception(wrapperSerializationResult.AsT1.Message);

        return wrapperSerializationResult.AsT0;
    }


    public OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> Deserialize(byte[] wrapperBytes)
    {        
        var wrapperDeserializationResult = byteSerailizer.Deserialize(wrapperBytes, wrapperMessageType);
        if(wrapperDeserializationResult.IsT1)
            throw new Exception(wrapperDeserializationResult.AsT1.Message);

        var wrapper = (ProtoMessageWrapper)wrapperDeserializationResult.Value;        
        OneOf<object, SerializationFailure> messageDataSerialiaztionResult;
        try
        {            
            messageDataSerialiaztionResult = byteSerailizer.Deserialize(wrapper.MessageData, wrapper.MessageType);
        }
        catch (Exception ex)
        {
            DeserializationFailureCase failure = new(wrapper.SessionId, wrapper.MessageCase, wrapper.MessageType, ex, $"{ex.Message}");
            return failure;
        }
        var message = messageDataSerialiaztionResult.AsT0;
        Type messageClrType = TypedToSimpleConverter.ConvertSimpleToType(wrapper.MessageType);
        switch (wrapper.MessageCase)
        {
            case MessageCase.CheckIn:
                var checkIn = (CheckInMessage)message;
                return checkIn;

            case MessageCase.Request:
                if (message is IMessage)
                {
                    RequestCase requestCase = new RequestCase(wrapper.SessionId, wrapper.MessageType, message, false, null);
                    return requestCase;
                }
                if (GenericsHelper.IsAssignableToGenericType(messageClrType, typeof(IMessage<>)))
                {
                    var responseType = GenericsHelper.GetTypeArgumentsFromParent(messageClrType, typeof(IMessage<>))[0];
                    RequestCase requestCase = new RequestCase(wrapper.SessionId, wrapper.MessageType, message, true, responseType);
                    return requestCase;
                }
                throw new Exception();

            case MessageCase.Response:
                ResponseCase responseCase = new ResponseCase(wrapper.SessionId, message, message.GetType());
                return responseCase;

            case MessageCase.Event:
                var eventCase = new EventCase(wrapper.SessionId, wrapper.MessageType, message);
                return eventCase;
        }

        throw new Exception();
    }
}
