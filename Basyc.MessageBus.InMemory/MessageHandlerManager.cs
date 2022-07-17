using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneOf;

namespace Basyc.MessageBus.Client.NetMQ
{
	public class MessageHandlerManager : IMessageHandlerManager
	{
		private readonly IServiceProvider serviceProvider;
		private readonly Dictionary<string, MessageHandlerInfo> handlerStorage = new();
		public MessageHandlerManager(IServiceProvider serviceProvider, IOptions<MessageHandlerManagerOptions> options)
		{
			this.serviceProvider = serviceProvider;


			foreach (var handler in options.Value.Handlers)
			{
				handlerStorage.Add(TypedToSimpleConverter.ConvertTypeToSimple(handler.MessageType), handler);
			}
		}

		public async Task<OneOf<object, Exception>> ConsumeMessage(string messageType, object? messageData, CancellationToken cancellationToken)
		{
			if (handlerStorage.TryGetValue(messageType, out var handlerInfo))
			{
				if (handlerInfo.HasResponse)
				{
					Type consumerType = typeof(IMessageHandler<,>).MakeGenericType(handlerInfo.MessageType, handlerInfo.ResponseType!);

					object handlerInstace = serviceProvider.GetRequiredService(consumerType)!;
					Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { messageData!, cancellationToken })!;
					await handlerResult;
					try
					{
						object taskResult = ((dynamic)handlerResult).Result!;
						return taskResult;
					}
					catch (Exception ex)
					{
						return ex;
					}

				}
				else
				{
					Type consumerType = typeof(IMessageHandler<>).MakeGenericType(handlerInfo.MessageType);
					object handlerInstace = serviceProvider.GetRequiredService(consumerType)!;
					Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { messageData!, cancellationToken })!;
					try
					{
						await handlerResult;
						return new VoidResult();
					}
					catch (Exception ex)
					{
						return ex;
					}
				}
			}

			throw new InvalidOperationException("Handler for this message not found");

		}

		public string[] GetConsumableMessageTypes()
		{
			return handlerStorage.Values.Select(x => TypedToSimpleConverter.ConvertTypeToSimple(x.MessageType)).ToArray();
		}
	}
}

