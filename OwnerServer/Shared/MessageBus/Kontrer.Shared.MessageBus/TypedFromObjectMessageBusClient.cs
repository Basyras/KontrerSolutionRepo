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

		BusTask ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
		{
			return objectBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple<TEvent>(), cancellationToken);
		}

		BusTask ITypedMessageBusClient.PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken)
		{
			return objectBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple<TEvent>(), eventData, cancellationToken);
		}

		BusTask<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
		{
			//var genericTask = objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(), cancellationToken);
			//return (TResponse)((dynamic)genericTask).Result;
			//return genericTask.ContinueWith(x=> (TResponse)x.Result);
			//return (TResponse)await genericTask.ConfigureAwait(false);

			var nestedBusTask = objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(), cancellationToken);
			return BusTask<TResponse>.FromBusTask(nestedBusTask, x => (TResponse)x);
		}

		BusTask<object> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
		{
			return objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple(requestType), cancellationToken);
		}

		BusTask<object> ITypedMessageBusClient.RequestAsync(Type requestType, object requestData, Type responseType, CancellationToken cancellationToken)
		{
			return objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple(requestType), requestData, cancellationToken);
		}

		BusTask<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest requestData, CancellationToken cancellationToken)
		{
			var innerBusTask = objectBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(), requestData, cancellationToken);
			return innerBusTask.ContinueWith(x => (OneOf<TResponse, ErrorMessage>)x);

		}


		BusTask ITypedMessageBusClient.SendAsync<TCommand>(CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple<TCommand>(), cancellationToken);
		}

		BusTask ITypedMessageBusClient.SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple<TCommand>(), command, cancellationToken);
		}

		BusTask ITypedMessageBusClient.SendAsync(Type commandType, object command, CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple(commandType), command, cancellationToken);
		}

		BusTask ITypedMessageBusClient.SendAsync(Type commandType, CancellationToken cancellationToken)
		{
			return objectBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple(commandType), cancellationToken);
		}

	}
}
