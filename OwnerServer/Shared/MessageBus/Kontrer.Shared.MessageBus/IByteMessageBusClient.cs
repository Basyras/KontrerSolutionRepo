using Basyc.MessageBus.Shared;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public interface IByteMessageBusClient
	{
		Task PublishAsync(string eventType, CancellationToken cancellationToken = default);
		Task PublishAsync(string eventType, byte[] eventData, CancellationToken cancellationToken = default);

		Task SendAsync(string commandType, CancellationToken cancellationToken = default);
		Task SendAsync(string commandType, byte[] commandData, CancellationToken cancellationToken = default);

		Task<ByteResponse> RequestAsync(string requestType, CancellationToken cancellationToken = default);
		Task<OneOf<ByteResponse, ErrorMessage>> RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken = default);
		Task<OneOf<ByteResponse, ErrorMessage>> RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken = default);

		Task StartAsync(CancellationToken cancellationToken = default);
	}
}
