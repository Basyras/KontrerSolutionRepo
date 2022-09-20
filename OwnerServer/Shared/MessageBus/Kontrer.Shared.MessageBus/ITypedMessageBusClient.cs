using Basyc.MessageBus.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public interface ITypedMessageBusClient : IDisposable
	{
		BusTask PublishAsync<TEvent>(CancellationToken cancellationToken = default)
			  where TEvent : class, IEventMessage, new();

		BusTask PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default)
			   where TEvent : notnull, IEventMessage;

		BusTask SendAsync<TCommand>(CancellationToken cancellationToken = default)
			 where TCommand : class, IMessage, new();
		BusTask SendAsync<TCommand>(TCommand commandData, CancellationToken cancellationToken = default)
			 where TCommand : notnull, IMessage;
		BusTask SendAsync(Type commandType, object commandData, CancellationToken cancellationToken = default);
		BusTask SendAsync(Type commandType, CancellationToken cancellationToken = default);

		BusTask<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
			 where TRequest : class, IMessage<TResponse>, new()
			 where TResponse : class;
		BusTask<object> RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken = default);
		BusTask<object> RequestAsync(Type requestType, object requestData, Type responseType, CancellationToken cancellationToken = default);
		BusTask<TResponse> RequestAsync<TRequest, TResponse>(TRequest requestData, CancellationToken cancellationToken = default)
			where TRequest : class, IMessage<TResponse>
			where TResponse : class;

		Task StartAsync(CancellationToken cancellationToken = default);
	}
}