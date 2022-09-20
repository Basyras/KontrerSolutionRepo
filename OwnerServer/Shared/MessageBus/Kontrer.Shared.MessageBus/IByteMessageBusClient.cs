using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public interface IByteMessageBusClient
	{
		BusTask PublishAsync(string eventType, CancellationToken cancellationToken = default);
		BusTask PublishAsync(string eventType, byte[] eventData, CancellationToken cancellationToken = default);

		BusTask SendAsync(string commandType, CancellationToken cancellationToken = default);
		BusTask SendAsync(string commandType, byte[] commandData, CancellationToken cancellationToken = default);

		BusTask<ByteResponse> RequestAsync(string requestType, CancellationToken cancellationToken = default);
		BusTask<ByteResponse> RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken = default);

		Task StartAsync(CancellationToken cancellationToken = default);
	}
}
