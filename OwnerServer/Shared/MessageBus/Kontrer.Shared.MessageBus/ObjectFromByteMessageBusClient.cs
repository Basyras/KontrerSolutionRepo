using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public class ObjectFromByteMessageBusClient : IObjectMessageBusClient
	{
		private readonly IByteMessageBusClient byteMessageBusClient;
		private readonly IObjectToByteSerailizer objectToByteSerailizer;

		public ObjectFromByteMessageBusClient(IByteMessageBusClient byteMessageBusClient, IObjectToByteSerailizer objectToByteSerailizer)
		{
			this.byteMessageBusClient = byteMessageBusClient;
			this.objectToByteSerailizer = objectToByteSerailizer;
		}

		public void Dispose()
		{
			if (byteMessageBusClient is IDisposable disposable)
				disposable.Dispose();
		}

		public Task PublishAsync(string eventType, CancellationToken cancellationToken = default)
		{
			return byteMessageBusClient.PublishAsync(eventType, cancellationToken);
		}

		public Task PublishAsync(string eventType, object eventData, CancellationToken cancellationToken = default)
		{
			var eventSerializationResult = objectToByteSerailizer.Serialize(eventData, eventType);
			return eventSerializationResult.Match(eventBytes =>
			{
				return byteMessageBusClient.PublishAsync(eventType, eventBytes, cancellationToken);
			},
			failure =>
			{
				throw new Exception(failure.Message);
			});
		}

		public async Task<object> RequestAsync(string requestType, CancellationToken cancellationToken = default)
		{
			var responseBytes = await byteMessageBusClient.RequestAsync(requestType, cancellationToken);
			var deserializatedResponseResult = objectToByteSerailizer.Deserialize(responseBytes.ResponseBytes, responseBytes.ResposneType);
			return deserializatedResponseResult.Match(
				   deserializedResponse => deserializedResponse,
				   error => throw new Exception(error.Message));
		}

		public Task<OneOf<object, ErrorMessage>> RequestAsync(string requestType, object requestData, CancellationToken cancellationToken = default)
		{
			var requestByteSerializationResult = objectToByteSerailizer.Serialize(requestData, requestType);
			return requestByteSerializationResult.Match<Task<OneOf<object, ErrorMessage>>>(async requestBytes =>
			{
				var requestResponseResult = await byteMessageBusClient.RequestAsync(requestType, requestBytes, cancellationToken);
				return requestResponseResult.Match(byteResponse =>
				{
					var responseDeserializationResult = objectToByteSerailizer.Deserialize(byteResponse.ResponseBytes, byteResponse.ResposneType);
					return responseDeserializationResult.Match(
						   deserializedResponse => deserializedResponse,
							 error1 => throw new Exception(error1.Message));
				},
				error2 => error2);
			},
			error3 => Task.FromResult<OneOf<object, ErrorMessage>>(new ErrorMessage(error3.Message)));
		}

		public Task SendAsync(string commandType, CancellationToken cancellationToken = default)
		{
			return byteMessageBusClient.SendAsync(commandType, cancellationToken);
		}

		public Task SendAsync(string commandType, object commandData, CancellationToken cancellationToken = default)
		{
			var commandByteSerializationResult = objectToByteSerailizer.Serialize(commandData, commandType);
			return commandByteSerializationResult.Match(requestBytes =>
			{
				return byteMessageBusClient.SendAsync(commandType, requestBytes, cancellationToken);

			},
			failure => throw new Exception(failure.Message));
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return byteMessageBusClient.StartAsync(cancellationToken);
		}
	}
}
