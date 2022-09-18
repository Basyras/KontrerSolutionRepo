﻿using Basyc.Serialization.Abstraction;
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
			var eventBytes = objectToByteSerailizer.Serialize(eventData, eventType);
			return byteMessageBusClient.PublishAsync(eventType, eventBytes, cancellationToken);
		}

		public async Task<object> RequestAsync(string requestType, CancellationToken cancellationToken = default)
		{
			var responseBytes = await byteMessageBusClient.RequestAsync(requestType, cancellationToken);
			return objectToByteSerailizer.Deserialize(responseBytes.ResponseBytes, responseBytes.ResposneType);
		}

		public BusTask<object> RequestAsync(string requestType, object requestData, CancellationToken cancellationToken = default)
		{
			var requestBytes = objectToByteSerailizer.Serialize(requestData, requestType);
			var innerBusTask = byteMessageBusClient.RequestAsync(requestType, requestBytes, cancellationToken);
			var busTask = BusTask<object>.FromBusTask(innerBusTask, byteResponse => objectToByteSerailizer.Deserialize(byteResponse.ResponseBytes, byteResponse.ResposneType));
			return busTask;
		}

		public Task SendAsync(string commandType, CancellationToken cancellationToken = default)
		{
			return byteMessageBusClient.SendAsync(commandType, cancellationToken);
		}

		public Task SendAsync(string commandType, object commandData, CancellationToken cancellationToken = default)
		{
			var requestBytes = objectToByteSerailizer.Serialize(commandData, commandType);
			return byteMessageBusClient.SendAsync(commandType, requestBytes, cancellationToken);
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return byteMessageBusClient.StartAsync(cancellationToken);
		}
	}
}
