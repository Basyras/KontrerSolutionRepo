using Basyc.MessageBus.HttpProxy.Shared.SignalR;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client.SignalR
{
	public class ProxyServerMethods : IProxyServerMethods
	{
		public ProxyServerMethods()
		{

		}
		public Task Request(RequestSignalRDTO proxyRequest)
		{

			return Task.CompletedTask;
		}
	}
}
