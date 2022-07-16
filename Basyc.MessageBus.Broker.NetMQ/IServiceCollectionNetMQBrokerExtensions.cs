using Basyc.MessageBus.NetMQ.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Broker.NetMQ
{
	public static class IServiceCollectionNetMQBrokerExtensions
	{
		public static IServiceCollection AddNetMQMessageBroker(this IServiceCollection services,
			int brokerServerPort = 5367, string brokerServerAddress = "localhost")
		{
			services.AddSingleton<IMessageBrokerServer, NetMQMessageBrokerServer>();
			services.AddSingleton<IWorkerRegistry, WorkerRegistry>();

			services.AddBasycSerialization()
				.SelectProtobufNet();

			services.AddSingleton<INetMQByteMessageSerializer, NetMQByteSerializer>();
			services.Configure<NetMQMessageBrokerServerOptions>(x =>
			{
				x.BrokerServerAddress = brokerServerAddress;
				x.BrokerServerPort = brokerServerPort;
			});
			return services;
		}

	}
}
