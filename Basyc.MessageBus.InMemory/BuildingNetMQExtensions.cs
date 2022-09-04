using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.Client.NetMQ;
using Basyc.MessageBus.Client.NetMQ.Sessions;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using Basyc.Shared.Helpers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class BuildingNetMQExtensions
{
	private const int defaultBrokerServerPort = 5367;
	private const string defaultBrokerServerAddress = "localhost";

	public static BusClientSetupProviderStage SelectNetMQProvider(this BusClientSetupProviderStage builder,
	   int? brokerServerPort) =>
		SelectNetMQProvider(builder, null, defaultBrokerServerPort, defaultBrokerServerAddress);

	public static BusClientSetupProviderStage SelectNetMQProvider(this BusClientSetupProviderStage builder,
		string? clientId = null, int brokerServerPort = defaultBrokerServerPort, string brokerServerAddress = defaultBrokerServerAddress)
	{
		var services = builder.services;
		AddClients(services);

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

		AddMessageHandlerManager(services);
		return builder;
	}

	private static void AddMessageHandlerManager(IServiceCollection services)
	{
		services.AddSingleton<IMessageHandlerManager, MessageHandlerManager>();
		services.Configure<MessageHandlerManagerOptions>(x =>
		{
			var messageHandlerTypes = services
				.Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<>)));

			foreach (var messageHandlerService in messageHandlerTypes)
			{
				Type handlerType = messageHandlerService.ImplementationType!;
				Type messageType = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<>))[0];
				MethodInfo handleMethodInfo = typeof(IMessageHandler<>).MakeGenericType(messageType).GetMethod(nameof(IMessageHandler<IMessage>.Handle))!;
				x.HandlerInfos.Add(new NetMQMessageHandlerInfo(TypedToSimpleConverter.ConvertTypeToSimple(messageType), handlerType, messageType, handleMethodInfo));
			}

			var messagesWithResponse = services
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
				x.HandlerInfos.Add(new NetMQMessageHandlerInfo(TypedToSimpleConverter.ConvertTypeToSimple(messageType), handlerType, messageType, responseType, handleWithResponseMethodInfo));
			}
		});
	}

	private static void AddClients(IServiceCollection services)
	{
		services.AddSingleton<IByteMessageBusClient, NetMQByteMessageBusClient>();
		services.AddSingleton<ITypedMessageBusClient, TypedFromByteMessageBusClient>();
		services.AddSingleton<IObjectMessageBusClient, ObjectFromByteMessageBusClient>();
	}
}
