using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public class ByteFromSimpleMessageBusClient : IByteMessageBusClient
	{
		private readonly ISimpleMessageBusClient simpleMessageBusClient;
		private readonly ISimpleToByteSerailizer byteSerailizer;

		public ByteFromSimpleMessageBusClient(ISimpleMessageBusClient simpleMessageBusClient, ISimpleToByteSerailizer byteSerailizer)
		{
			this.simpleMessageBusClient = simpleMessageBusClient;
			this.byteSerailizer = byteSerailizer;
		}
		public Task PublishAsync(string eventType, CancellationToken cancellationToken = default)
		{
			return simpleMessageBusClient.PublishAsync(eventType, cancellationToken);
		}

		public Task PublishAsync(string eventType, byte[] eventData, CancellationToken cancellationToken = default)
		{
			return simpleMessageBusClient.PublishAsync(eventType, eventData, cancellationToken);
		}

		public async Task<byte[]> RequestAsync(string requestType, CancellationToken cancellationToken = default)
		{
			var resposne = await simpleMessageBusClient.RequestAsync(requestType, cancellationToken);
			return (byte[])resposne;
		}

		public async Task<OneOf<byte[], ErrorMessage>> RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken = default)
		{
			var requestResult = await simpleMessageBusClient.RequestAsync(requestType, requestData, cancellationToken);
			return requestResult.Match<OneOf<byte[], ErrorMessage>>(resultObject =>
			{
				if (resultObject is byte[] bytes)
					return bytes;

				return byteSerailizer.Serialize(resultObject, requestType).AsT0;
			}, error => error);
		}

		public Task SendAsync(string commandType, CancellationToken cancellationToken = default)
		{
			return simpleMessageBusClient.SendAsync(commandType, cancellationToken);
		}

		public Task SendAsync(string commandType, byte[] commandData, CancellationToken cancellationToken = default)
		{
			return simpleMessageBusClient.SendAsync(commandType, commandData, cancellationToken);
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return simpleMessageBusClient.StartAsync(cancellationToken);
		}
	}
}
