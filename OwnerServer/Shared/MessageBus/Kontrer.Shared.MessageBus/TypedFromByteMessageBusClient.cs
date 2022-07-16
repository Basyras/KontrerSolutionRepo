﻿using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public class TypedFromByteMessageBusClient : ITypedMessageBusClient
	{
		private readonly IByteMessageBusClient byteMessageBusClient;
		private readonly IObjectToByteSerailizer objectToByteSerailizer;

		public TypedFromByteMessageBusClient(IByteMessageBusClient byteMessageBusClient, IObjectToByteSerailizer objectToByteSerailizer)
		{
			this.byteMessageBusClient = byteMessageBusClient;
			this.objectToByteSerailizer = objectToByteSerailizer;
		}

		public void Dispose()
		{
			if (byteMessageBusClient is IDisposable disposable)
				disposable.Dispose();
		}

		public Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default) where TEvent : notnull, IEventMessage
		{
			return byteMessageBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple(typeof(TEvent)), cancellationToken);
		}

		public async Task<object> RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken = default)
		{
			var requestTypeString = TypedToSimpleConverter.ConvertTypeToSimple(requestType);
			var responseTypeString = TypedToSimpleConverter.ConvertTypeToSimple(responseType);

			var requestResponse = await byteMessageBusClient.RequestAsync(requestTypeString, cancellationToken);
			var responseDeserializationResult = objectToByteSerailizer.Deserialize(requestResponse.ResponseBytes, requestResponse.ResposneType);
			return responseDeserializationResult.Match(
				   deserializedResponse => deserializedResponse,
					 error => throw new Exception(error.Message));
		}

		public Task<object> RequestAsync(Type requestType, object requestData, Type responseType, CancellationToken cancellationToken = default)
		{
			var requestTypeString = TypedToSimpleConverter.ConvertTypeToSimple(requestType);
			var responseTypeString = TypedToSimpleConverter.ConvertTypeToSimple(responseType);

			var requestByteSerializationResult = objectToByteSerailizer.Serialize(requestData, requestTypeString);
			return requestByteSerializationResult.Match(async requestBytes =>
			{
				var requestResponseResult = await byteMessageBusClient.RequestAsync(requestTypeString, requestBytes, cancellationToken);
				return requestResponseResult.Match(byteResponse =>
				{
					var responseDeserializationResult = objectToByteSerailizer.Deserialize(byteResponse.ResponseBytes, byteResponse.ResposneType);
					return responseDeserializationResult.Match(
						   deserializedResponse => deserializedResponse,
							 error => throw new Exception(error.Message));
				},
				error => throw new Exception(error.Message));

			},
			failure => throw new Exception(failure.Message));
		}

		public Task SendAsync<TCommand>(TCommand commandData, CancellationToken cancellationToken = default) where TCommand : notnull, IMessage
		{
			var commandTypeString = TypedToSimpleConverter.ConvertTypeToSimple(typeof(TCommand));
			return byteMessageBusClient.SendAsync(commandTypeString, cancellationToken);
		}

		public Task SendAsync(Type commandType, object commandData, CancellationToken cancellationToken = default)
		{
			var commandTypeString = TypedToSimpleConverter.ConvertTypeToSimple(commandType);
			var commandByteSerializationResult = objectToByteSerailizer.Serialize(commandData, commandTypeString);
			return commandByteSerializationResult.Match(requestBytes =>
			{
				return byteMessageBusClient.SendAsync(commandTypeString, requestBytes, cancellationToken);

			},
			failure => throw new Exception(failure.Message));
		}

		public Task SendAsync(Type commandType, CancellationToken cancellationToken = default)
		{
			var commandTypeString = TypedToSimpleConverter.ConvertTypeToSimple(commandType);
			return byteMessageBusClient.SendAsync(commandTypeString, cancellationToken);
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return byteMessageBusClient.StartAsync(cancellationToken);
		}

		Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
		{
			var eventTypeString = TypedToSimpleConverter.ConvertTypeToSimple(typeof(TEvent));
			return byteMessageBusClient.PublishAsync(eventTypeString, cancellationToken);

		}

		async Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
		{
			return (TResponse)(await RequestAsync(typeof(TRequest), typeof(TResponse), cancellationToken));

		}

		async Task<OneOf<TResponse, ErrorMessage>> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest requestData, CancellationToken cancellationToken)
		{
			return (TResponse)(await RequestAsync(typeof(TRequest), requestData, typeof(TResponse), cancellationToken));
		}

		Task ITypedMessageBusClient.SendAsync<TCommand>(CancellationToken cancellationToken)
		{
			return SendAsync(typeof(TCommand), cancellationToken);
		}
	}
}
