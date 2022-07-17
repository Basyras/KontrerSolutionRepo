using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.Client.NetMQ;
using Basyc.MessageBus.Client.NetMQ.Sessions;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Shared.Helpers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class BuildingNetMQExtensions
{
	public static BusClientSelectClientProviderStage AddNetMQClient(this BusClientSelectClientProviderStage builder,
	   int? brokerServerPort) =>
		AddNetMQClient(builder, brokerServerPort);

	public static BusClientSelectClientProviderStage UseNetMQProvider(this BusClientSelectClientProviderStage builder,
		string? clientId = null, int brokerServerPort = 5367, string brokerServerAddress = "localhost")
	{
		var services = builder.services;
		//switch (builder.MessageType)
		//{
		//	case MessageTypeOptions.Typed:
		//		services.AddSingleton<IByteMessageBusClient, NetMQObjectMessageBusClient>();
		//		services.AddSingleton<ITypedMessageBusClient, TypedFromByteMessageBusClient>();
		//		//services.AddSingleton<IObjectMessageBusClient, ObjectFromByteMessageBusClient>();
		//		break;
		//	case MessageTypeOptions.Object:
		//		throw new NotSupportedException();
		//	case MessageTypeOptions.Byte:
		//		services.AddSingleton<IByteMessageBusClient, NetMQObjectMessageBusClient>();
		//		services.AddSingleton<ITypedMessageBusClient, TypedFromByteMessageBusClient>();
		//		//services.AddSingleton<IObjectMessageBusClient, ObjectFromByteMessageBusClient>();
		//		break;
		//}

		services.AddSingleton<IByteMessageBusClient, NetMQByteMessageBusClient>();
		services.AddSingleton<ITypedMessageBusClient, TypedFromByteMessageBusClient>();
		services.AddSingleton<IObjectMessageBusClient, ObjectFromByteMessageBusClient>();


		services.AddSingleton<ISessionManager<NetMQSessionResult>, InMemorySessionManager<NetMQSessionResult>>();
		services.Configure<NetMQMessageBusClientOptions>(x =>
		{
			x.BrokerServerAddress = brokerServerAddress;
			x.BrokerServerPort = brokerServerPort;
			x.WorkerId = clientId;
		});

		services.AddBasycSerialization()
			.SelectProtobufNet();

		services.AddSingleton<INetMQMessageWrapper, NetMQMessageWrapper>();
		var areMessagesByte = services.FirstOrDefault(x => x.ServiceType == typeof(IByteMessageBusClient)) == null;


		services.AddSingleton<IMessageHandlerManager, MessageHandlerManager>();
		services.Configure<MessageHandlerManagerOptions>(x =>
		{
			var messageHandlerTypes = builder.services
				.Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<>)));

			foreach (var messageHandlerService in messageHandlerTypes)
			{
				Type handlerType = messageHandlerService.ImplementationType!;
				Type messageType = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<>))[0];
				MethodInfo handleMethodInfo = typeof(IMessageHandler<>).MakeGenericType(messageType).GetMethod(nameof(IMessageHandler<IMessage>.Handle))!;
				x.Handlers.Add(new MessageHandlerInfo(handlerType, messageType, handleMethodInfo));
			}

			var messagesWithResponse = builder.services
				.Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<,>)));

			foreach (var messageHandlerServiceWithResponse in messagesWithResponse)
			{
				Type handlerType = messageHandlerServiceWithResponse.ImplementationType!;
				Type[] typeArguments = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<,>));
				Type messageType = typeArguments[0];
				Type responseType = typeArguments[1];
				MethodInfo handleWithResponseMethodInfo = typeof(IMessageHandler<,>)
				.MakeGenericType(messageType, responseType)
				.GetMethod(nameof(IMessageHandler<IMessage<object>, object>.Handle))!;
				x.Handlers.Add(new MessageHandlerInfo(handlerType, messageType, responseType, handleWithResponseMethodInfo));
			}
		});
		return builder;
	}


}
