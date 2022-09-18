﻿using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.HttpProxy.Client.Http
{
	public class SetupHttpProxyStage : BuilderStageBase
	{
		public SetupHttpProxyStage(IServiceCollection services) : base(services)
		{
		}

		public SetupHttpProxyStage SetProxyServerUri(Uri hostUri)
		{
			services.Configure<HttpProxyObjectMessageBusClientOptions>(x => x.ProxyHostUri = hostUri);
			return this;
		}
	}
}