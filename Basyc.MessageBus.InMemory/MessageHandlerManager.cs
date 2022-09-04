using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;

namespace Basyc.MessageBus.Client.NetMQ
{
	public class MessageHandlerManager : IMessageHandlerManager
	{
		private readonly IServiceProvider serviceProvider;
		///private readonly Dictionary<string, NetMQMessageHandlerInfo> supportedHandlersMap = new();
		private readonly Dictionary<string, HandlerMetadata> handlerTypesCacheMap = new();
		public MessageHandlerManager(IServiceProvider serviceProvider, IOptions<MessageHandlerManagerOptions> options)
		{
			this.serviceProvider = serviceProvider;

			foreach (var handlerInfo in options.Value.HandlerInfos)
			{
				//supportedHandlersMap.Add(handlerInfo.MessageSimpleType, handlerInfo);
				Type handlerInterfaceType;
				Type handlerType;

				if (handlerInfo.HasResponse)
				{
					handlerInterfaceType = typeof(IMessageHandler<,>);
					handlerType = handlerInterfaceType.MakeGenericType(handlerInfo.MessageType, handlerInfo.ResponseType!);

				}
				else
				{
					handlerInterfaceType = typeof(IMessageHandler<>);
					handlerType = handlerInterfaceType.MakeGenericType(handlerInfo.MessageType);
				}

				Type handlerLoggerType = typeof(ILogger<>).MakeGenericType(handlerType);
				ILogger handlerLogger = (ILogger)serviceProvider.GetRequiredService(handlerLoggerType);
				HandlerMetadata newHandlerMetadata = new(handlerInfo, handlerType, handlerLogger);
				handlerTypesCacheMap.Add(handlerInfo.MessageSimpleType, newHandlerMetadata);
			}

		}

		public async Task<OneOf<object, Exception>> ConsumeMessage(string messageType, object? messageData, CancellationToken cancellationToken)
		{
			//if (supportedHandlersMap.TryGetValue(messageType, out var handlerInfo) is false)
			//{
			//	throw new InvalidOperationException("Handler for this message not found");
			//}

			//if (handlerTypesCacheMap.TryGetValue(messageType, out var handlerMetadata) is false)
			//{
			//	var handlerInterfaceType = handlerInfo.HasResponse ? typeof(IMessageHandler<,>) : typeof(IMessageHandler<>);
			//	Type handlerType = handlerInterfaceType.MakeGenericType(handlerInfo.MessageType, handlerInfo.ResponseType!);
			//	Type handlerLoggerType = typeof(ILogger<>).MakeGenericType(handlerType);
			//	ILogger handlerLogger = (ILogger)serviceProvider.GetRequiredService(handlerLoggerType);
			//	HandlerMetadata newHandlerMetadata = new(handlerInfo, handlerType, handlerLogger);
			//	handlerTypesCacheMap.Add(messageType, newHandlerMetadata);
			//	handlerMetadata = newHandlerMetadata;
			//}

			if (handlerTypesCacheMap.TryGetValue(messageType, out var handlerMetadata) is false)
			{
				throw new InvalidOperationException("Handler for this message not found");
			}

			object handler = serviceProvider.GetRequiredService(handlerMetadata.HandlerRuntimeType)!;

			//using var scope = handlerMetadata.HandlerLogger.BeginScope("");
			Task handlerResult = (Task)handlerMetadata.HandlerInfo.HandleMethodInfo.Invoke(handler, new object[] { messageData!, cancellationToken })!;
			try
			{
				if (handlerMetadata.HandlerInfo.HasResponse)
				{
					object taskResult = ((dynamic)handlerResult).Result!;
					return taskResult;

				}
				else
				{
					await handlerResult;
					return new VoidResult();
				}
			}
			catch (Exception ex)
			{
				return ex;
			}


			//if (handlerInfo.HasResponse)
			//{
			//	Type handlerType = typeof(IMessageHandler<,>).MakeGenericType(handlerInfo.MessageType, handlerInfo.ResponseType!);
			//	handlerTypesCacheMap.Add(messageType, handlerType);
			//	object handler = serviceProvider.GetRequiredService(handlerType)!;
			//	var handlerLogger = GetHandlerLogger(handlerType);

			//	Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handler, new object[] { messageData!, cancellationToken })!;
			//	await handlerResult;

			//	try
			//	{
			//		object taskResult = ((dynamic)handlerResult).Result!;
			//		return taskResult;
			//	}
			//	catch (Exception ex)
			//	{
			//		return ex;
			//	}
			//}
			//else
			//{
			//	Type handlerType = typeof(IMessageHandler<>).MakeGenericType(handlerInfo.MessageType);
			//	handlerTypesCacheMap.Add(messageType, handlerType);

			//	object handler = serviceProvider.GetRequiredService(handlerType)!;
			//	Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handler, new object[] { messageData!, cancellationToken })!;
			//	try
			//	{
			//		await handlerResult;
			//		return new VoidResult();
			//	}
			//	catch (Exception ex)
			//	{
			//		return ex;
			//	}
			//}

		}

		private ILogger GetHandlerLogger(Type handlerType)
		{
			Type handlerLoggerType = typeof(ILogger<>).MakeGenericType(handlerType);
			var handlerLogger = serviceProvider.GetRequiredService(handlerLoggerType);
			return (ILogger)handlerLogger;
		}

		public string[] GetConsumableMessageTypes()
		{
			return handlerTypesCacheMap.Values
				.Select(handlerMetadata => handlerMetadata.HandlerInfo.MessageSimpleType)
				.ToArray();
		}

		private record HandlerMetadata(NetMQMessageHandlerInfo HandlerInfo, Type HandlerRuntimeType, ILogger HandlerLogger);
	}
}

