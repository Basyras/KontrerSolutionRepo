using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Shared.SignalR
{
	public interface IProxyClientMethods
	{
		Task ReceiveRequestResultMetadata(RequestMetadataSignalRDTO requestMetadata);
		Task ReceiveRequestResult(RequestResponseSignalRDTO response);
		Task ReceiveRequestFailed(RequestFailedSignalRDTO requestFailed);
	}
}