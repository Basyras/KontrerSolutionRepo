using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public sealed class TypedFromObjectMessageBusClient : ITypedMessageBusClient
	{
		private readonly IObjectMessageBusClient objectBusClient;

		public TypedFromObjectMessageBusClient(IObjectMessageBusClient messageBusClient)
		{
			this.objectBusClient = messageBusClient;
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return objectBusClient.StartAsync(cancellationToken);
		}

		void IDisposable.Dispose()
		{
			objectBusClient.Dispose();
		}

		Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
		{
			return objectBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple<TEvent>(), cancellationToken);
		}

		Task ITypedMessageBusClient.PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken)
		{
			return objectBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple<TEvent>(), eventData, cancellationToken);
		}

		async Task<OneOf<TResponse, ErrorMessage>> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
		{
			var genericTask = objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(), cancellationToken);
			//return (TResponse)((dynamic)genericTask).Result;
			//return genericTask.ContinueWith(x=> (TResponse)x.Result);
			return (TResponse)await genericTask.ConfigureAwait(false);

		}

		async Task<OneOf<object, ErrorMessage>> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
		{
			var genericTask = await objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple(requestType), cancellationToken);
			return genericTask;
		}

		BusTask<object> ITypedMessageBusClient.RequestAsync(Type requestType, object requestData, Type responseType, CancellationToken cancellationToken)
		{
			var busTask = objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple(requestType), requestData, cancellationToken);
			return busTask;
		}

		async Task<OneOf<TResponse, ErrorMessage>> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest requestData, CancellationToken cancellationToken)
		{
			var genericTask = objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(), requestData, cancellationToken).Task;
			var responseData = await genericTask.ConfigureAwait(false);
			if (responseData.Value is ErrorMessage fail)
			{
				return fail;
			}
			else
			{
				return (TResponse)responseData.Value;
			}
		}


		Task ITypedMessageBusClient.SendAsync<TCommand>(CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple<TCommand>(), cancellationToken);
		}

		Task ITypedMessageBusClient.SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple<TCommand>(), command, cancellationToken);
		}

		Task ITypedMessageBusClient.SendAsync(Type commandType, object command, CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple(commandType), command, cancellationToken);
		}

		Task ITypedMessageBusClient.SendAsync(Type commandType, CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple(commandType), cancellationToken);
		}

	}
}
