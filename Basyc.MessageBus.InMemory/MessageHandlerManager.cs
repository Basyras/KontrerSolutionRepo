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
		private readonly Dictionary<string, HandlerMetadata> handlerTypesCacheMap = new();
		public MessageHandlerManager(IServiceProvider serviceProvider, IOptions<MessageHandlerManagerOptions> options)
		{
			this.serviceProvider = serviceProvider;

			foreach (var handlerInfo in options.Value.HandlerInfos)
			{
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

				Type handlerLoggerType = typeof(ILogger<>).MakeGenericType(handlerInfo.HandlerType);
				ILogger handlerLogger = (ILogger)serviceProvider.GetRequiredService(handlerLoggerType);
				HandlerMetadata newHandlerMetadata = new(handlerInfo, handlerType, handlerLogger);
				handlerTypesCacheMap.Add(handlerInfo.MessageSimpleType, newHandlerMetadata);
			}

		}

		public async Task<OneOf<object, Exception>> ConsumeMessage(string messageType, object? messageData, CancellationToken cancellationToken)
		{
			if (handlerTypesCacheMap.TryGetValue(messageType, out var handlerMetadata) is false)
			{
				throw new InvalidOperationException("Handler for this message not found");
			}

			object handler = serviceProvider.GetRequiredService(handlerMetadata.HandlerRuntimeType)!;
			Task handlerResultTask = (Task)handlerMetadata.HandlerInfo.HandleMethodInfo.Invoke(handler, new object[] { messageData!, cancellationToken })!;
			object? handlerResult;
			try
			{
				if (handlerMetadata.HandlerInfo.HasResponse)
				{
					object taskResult = ((dynamic)handlerResultTask).Result!;

					handlerResult = taskResult;

				}
				else
				{
					await handlerResultTask;
					handlerResult = new VoidResult();
				}
			}
			catch (Exception ex)
			{
				return ex;
			}

			return handlerResult;
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

