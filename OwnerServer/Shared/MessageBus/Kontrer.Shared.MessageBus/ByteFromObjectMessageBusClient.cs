using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public class ByteFromObjectMessageBusClient : IByteMessageBusClient
	{
		private readonly IObjectMessageBusClient objectMessageBusClient;
		private readonly IObjectToByteSerailizer byteSerailizer;

		public ByteFromObjectMessageBusClient(IObjectMessageBusClient objectMessageBusClient, IObjectToByteSerailizer byteSerailizer)
		{
			this.objectMessageBusClient = objectMessageBusClient;
			this.byteSerailizer = byteSerailizer;
		}
		public Task PublishAsync(string eventType, CancellationToken cancellationToken = default)
		{
			return objectMessageBusClient.PublishAsync(eventType, cancellationToken);
		}

		public Task PublishAsync(string eventType, byte[] eventData, CancellationToken cancellationToken = default)
		{
			return objectMessageBusClient.PublishAsync(eventType, eventData, cancellationToken);
		}

		public async Task<ByteResponse> RequestAsync(string requestType, CancellationToken cancellationToken = default)
		{
			var resposne = await objectMessageBusClient.RequestAsync(requestType, cancellationToken);
			return new ByteResponse((byte[])resposne, "unknown");
		}

		public BusTask<ByteResponse> RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken = default)
		{
			var innerBusTask = objectMessageBusClient.RequestAsync(requestType, requestData, cancellationToken);
			//return requestResult.Match<OneOf<ByteResponse, ErrorMessage>>(resultObject =>
			//{
			//	if (resultObject is byte[] bytes)
			//		return new ByteResponse(bytes, "unknown");

			//	//return byteSerailizer.Serialize(resultObject, requestType).AsT0;
			//	throw new System.Exception("Does not know how to serialize");
			//}, error => error);
			return BusTask<ByteResponse>.FromBusTask(innerBusTask, nestedValue =>
			{
				if (nestedValue is byte[] bytes)
					return new ByteResponse(bytes, "unknown");
				else
					return new ErrorMessage("Does not know how to serialize");
			});
		}

		public Task SendAsync(string commandType, CancellationToken cancellationToken = default)
		{
			return objectMessageBusClient.SendAsync(commandType, cancellationToken);
		}

		public Task SendAsync(string commandType, byte[] commandData, CancellationToken cancellationToken = default)
		{
			return objectMessageBusClient.SendAsync(commandType, commandData, cancellationToken);
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return objectMessageBusClient.StartAsync(cancellationToken);
		}
	}
}
