using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Shared.SignalR
{
	public interface IProxyServerMethods
	{
		Task Request(RequestSignalRDTO proxyRequest);
	}
}