using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.HttpProxy.Client;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class SetupProxyStageProxyExtensions
	{
		public static ClientProxySetupStage UseHttpProxy(this BusClientSetupProxyStage builder)
		{
			builder.services.AddBasycSerialization()
				.SelectProtobufNet();
			builder.services.AddSingleton(new HttpClient());
			builder.services.AddSingleton<IObjectMessageBusClient, HttpProxyObjectMessageBusClient>();
			builder.services.AddSingleton<ITypedMessageBusClient, TypedFromObjectMessageBusClient>();

			return new ClientProxySetupStage(builder.services);
		}
	}
}