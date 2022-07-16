using Basyc.MessageBus.Client;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.HttpProxy.Client
{
	public class MessageBusHttpProxyClientBuilder
	{
		private readonly IServiceCollection services;

		public MessageBusHttpProxyClientBuilder(IServiceCollection services)
		{
			services.AddSingleton<IObjectMessageBusClient, ProxyClientSimpleMessageBusClient>();
			services.AddSingleton<ITypedMessageBusClient, TypedFromObjectMessageBusClient>();
			this.services = services;
		}

		//public MessageBusHttpProxyClientBuilder UseSerializer<TSerializer>() where TSerializer : class, IRequestSerializer
		//{
		//    services.AddSingleton<TSerializer>();
		//    return this;
		//}

		public MessageBusHttpProxyClientBuilder SetProxyServerUri(Uri hostUri)
		{
			services.Configure<MessageBusHttpProxyClientOptions>(x => x.ProxyHostUri = hostUri);
			return this;
		}
	}
}