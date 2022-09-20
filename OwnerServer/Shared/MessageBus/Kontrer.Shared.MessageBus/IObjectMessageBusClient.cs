using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public interface IObjectMessageBusClient : IDisposable
	{
		BusTask PublishAsync(string eventType, CancellationToken cancellationToken = default);
		BusTask PublishAsync(string eventType, object eventData, CancellationToken cancellationToken = default);

		BusTask SendAsync(string commandType, CancellationToken cancellationToken = default);
		BusTask SendAsync(string commandType, object commandData, CancellationToken cancellationToken = default);

		BusTask<object> RequestAsync(string requestType, CancellationToken cancellationToken = default);
		BusTask<object> RequestAsync(string requestType, object requestData, CancellationToken cancellationToken = default);

		Task StartAsync(CancellationToken cancellationToken = default);
	}
}
