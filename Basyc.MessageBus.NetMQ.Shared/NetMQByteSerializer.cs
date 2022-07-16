using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Basyc.Shared.Helpers;
using OneOf;

namespace Basyc.MessageBus.NetMQ.Shared;

public class NetMQByteSerializer : INetMQByteMessageSerializer
{
	private readonly IObjectToByteSerailizer objectToByteSerializer;
	private static readonly string wrapperMessageType = TypedToSimpleConverter.ConvertTypeToSimple<ProtoMessageWrapper>();

	public NetMQByteSerializer(IObjectToByteSerailizer byteSerailizer)
	{
		this.objectToByteSerializer = byteSerailizer;

	}

	public byte[] Serialize(object? messageData, string messageType, int sessionId, MessageCase messageCase)
	{
		byte[]? messageBytes = null;

		if (messageData is byte[] bytes)
		{
			messageBytes = bytes;
		}
		else
		{
			var messageDataSeriliazationResult = objectToByteSerializer.Serialize(messageData, messageType);
			messageDataSeriliazationResult.Switch(
				   bytes => { messageBytes = bytes; },
				   error => throw new Exception(error.Message));
		}

		if (messageBytes is null)
			throw new Exception();

		var wrapperMessageData = new ProtoMessageWrapper(sessionId, messageCase, messageType, messageBytes);
		var wrapperSerializationResult = objectToByteSerializer.Serialize(wrapperMessageData, wrapperMessageType);
		if (wrapperSerializationResult.IsT1)
			throw new Exception(wrapperSerializationResult.AsT1.Message);

		return wrapperSerializationResult.AsT0;
	}


	public OneOf<CheckInMessage, RequestCase, ResponseCase, EventCase, DeserializationFailureCase> Deserialize(byte[] wrapperBytes)
	{
		var wrapperDeserializationResult = objectToByteSerializer.Deserialize(wrapperBytes, wrapperMessageType);
		if (wrapperDeserializationResult.IsT1)
			throw new Exception(wrapperDeserializationResult.AsT1.Message);

		var wrapper = (ProtoMessageWrapper)wrapperDeserializationResult.Value;
		Type messageClrType = TypedToSimpleConverter.ConvertSimpleToType(wrapper.MessageType);

		OneOf<object, SerializationFailure> messageDataSerialiaztionResult;
		object messageData;
		switch (wrapper.MessageCase)
		{
			case MessageCase.CheckIn:
				try
				{
					messageDataSerialiaztionResult = objectToByteSerializer.Deserialize(wrapper.MessageData, wrapper.MessageType);
				}
				catch (Exception ex)
				{
					DeserializationFailureCase failure = new(wrapper.SessionId, wrapper.MessageCase, wrapper.MessageType, ex, $"{ex.Message}");
					return failure;
				}
				messageData = messageDataSerialiaztionResult.AsT0;
				var checkIn = (CheckInMessage)messageData;
				return checkIn;

			case MessageCase.Request:
				try
				{
					messageDataSerialiaztionResult = objectToByteSerializer.Deserialize(wrapper.MessageData, wrapper.MessageType);
				}
				catch (Exception ex)
				{
					DeserializationFailureCase failure = new(wrapper.SessionId, wrapper.MessageCase, wrapper.MessageType, ex, $"{ex.Message}");
					return failure;
				}
				messageData = messageDataSerialiaztionResult.AsT0;
				if (messageData is IMessage)
				{
					RequestCase requestCase = new RequestCase(wrapper.SessionId, wrapper.MessageType, messageData, false, null);
					return requestCase;
				}
				if (GenericsHelper.IsAssignableToGenericType(messageClrType, typeof(IMessage<>)))
				{
					var responseType = GenericsHelper.GetTypeArgumentsFromParent(messageClrType, typeof(IMessage<>))[0];
					RequestCase requestCase = new RequestCase(wrapper.SessionId, wrapper.MessageType, messageData, true, responseType);
					return requestCase;
				}
				throw new Exception();

			case MessageCase.Response:
				ResponseCase responseCase = new ResponseCase(wrapper.SessionId, wrapper.MessageData, wrapper.MessageType);
				return responseCase;

			case MessageCase.Event:
				try
				{
					messageDataSerialiaztionResult = objectToByteSerializer.Deserialize(wrapper.MessageData, wrapper.MessageType);
				}
				catch (Exception ex)
				{
					DeserializationFailureCase failure = new(wrapper.SessionId, wrapper.MessageCase, wrapper.MessageType, ex, $"{ex.Message}");
					return failure;
				}
				messageData = messageDataSerialiaztionResult.AsT0;
				var eventCase = new EventCase(wrapper.SessionId, wrapper.MessageType, messageData);
				return eventCase;
			default:
				throw new Exception();
		}
	}
}
