using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.HttpProxy.Client
{
	public class ClientProxySetupStage : BuilderStageBase
	{
		public ClientProxySetupStage(IServiceCollection services) : base(services)
		{
		}

		public ClientProxySetupStage SetProxyServerUri(Uri hostUri)
		{
			services.Configure<HttpProxyObjectMessageBusClientOptions>(x => x.ProxyHostUri = hostUri);
			return this;
		}
	}
}