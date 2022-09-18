using System;

namespace Basyc.MessageBus.HttpProxy.Client.Http
{
	public class HttpProxyObjectMessageBusClientOptions
	{
		public HttpProxyObjectMessageBusClientOptions()
		{
		}

		public Uri ProxyHostUri { get; set; }
	}
}