﻿using Basyc.MessageBus.HttpProxy.Server.Asp;
using Basyc.Serialization.Abstraction;
using Basyc.Serialization.ProtobufNet;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceCollectionMessageBusServerExtensions
	{
		public static IServiceCollection AddMessageBusProxy(this IServiceCollection services)
		{
			//services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
			services.AddSingleton<ITypedByteSerializer, ProtobufByteSerializer>();
			services.AddSingleton<IObjectToByteSerailizer, ObjectFromTypedByteSerializer>();
			services.AddSingleton<ProxyHttpRequestHandler>();
			return services;
		}
	}
}