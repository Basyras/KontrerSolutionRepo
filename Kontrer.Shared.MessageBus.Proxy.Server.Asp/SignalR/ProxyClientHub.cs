using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Server.Asp.SignalR
{
	public class ProxyClientHub : Hub<IProxyClientMethods>, IProxyServerMethods
	{
		private readonly IByteMessageBusClient messageBus;

		public ProxyClientHub(IByteMessageBusClient messageBus)
		{
			this.messageBus = messageBus;
		}

		public async Task Request(RequestSignalRDTO proxyRequest)
		{
			if (proxyRequest.HasResponse)
			{
				var busTask = messageBus.RequestAsync(proxyRequest.MessageType, proxyRequest.MessageBytes);
				await Clients.Caller.ReceiveRequestResultMetadata(new RequestMetadataSignalRDTO(busTask.SessionId));
				var busTaskValue = await busTask.Task;
				await busTaskValue.Match(
					async byteResponse =>
					{
						var response = new RequestResponseSignalRDTO(proxyRequest.MessageType, proxyRequest.HasResponse, byteResponse.ResponseBytes, byteResponse.ResposneType);
						await Clients.Caller.ReceiveRequestResult(response);
					},
					async busRequestError =>
					{
						RequestFailedSignalRDTO failure = new RequestFailedSignalRDTO(busTask.SessionId, busRequestError.Message);
						await Clients.Caller.ReceiveRequestFailed(failure);
					});
			}
			else
			{
				await messageBus.SendAsync(proxyRequest.MessageType, proxyRequest.MessageBytes);
			}
		}
	}
}
