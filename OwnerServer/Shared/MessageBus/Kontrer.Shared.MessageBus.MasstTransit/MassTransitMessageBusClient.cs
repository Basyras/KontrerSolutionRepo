﻿using Basyc.MessageBus.Shared;
using MassTransit;
using OneOf;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.MasstTransit
{
	public class MassTransitMessageBusClient : ITypedMessageBusClient
	{
		private readonly IBusControl massTransitBus;

		public MassTransitMessageBusClient(IBusControl massTransitBus)
		{
			this.massTransitBus = massTransitBus;
		}

		Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
		{
			return massTransitBus.Publish<TEvent>(cancellationToken);
		}

		Task ITypedMessageBusClient.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
		{
			return massTransitBus.Publish(data, cancellationToken);
		}

		async Task<OneOf<TResponse, ErrorMessage>> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
		{
			var response = await massTransitBus.Request<TRequest, TResponse>(cancellationToken);
			return response.Message;
		}

		async Task<OneOf<TResponse, ErrorMessage>> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
		{
			var response = await massTransitBus.Request<TRequest, TResponse>(request, cancellationToken);
			return response.Message;
		}

		async Task<OneOf<object, ErrorMessage>> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
		{
			var bus = (ITypedMessageBusClient)this;
			return await bus.RequestAsync(requestType, Activator.CreateInstance(requestType), responseType, cancellationToken);
		}

		public async Task<OneOf<object, ErrorMessage>> RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken)
		{
			var conType = typeof(SendContext<>).MakeGenericType(requestType);
			var actionType = typeof(Action<>).MakeGenericType(conType);
			var methodParameterTypes = new Type[] { typeof(IBus), requestType, typeof(CancellationToken), typeof(RequestTimeout), actionType };
			//MassTransit does not have Request variant that accepts type as parameter (not type parameter)
			//MethodInfo requestMethodInfo = typeof(MassTransit.RequestExtensions).GetMethod(nameof(MassTransit.RequestExtensions.Request), BindingFlags.Public | BindingFlags.Static, null, methodParameterTypes, null);
			MethodInfo requestMethodInfo = typeof(RequestExtensions).GetMethods().Where(x => x.Name == nameof(RequestExtensions.Request)).Skip(2).First();
			var parameters = requestMethodInfo.GetParameters();
			MethodInfo genericMethod = requestMethodInfo.MakeGenericMethod(requestType, responseType);

			var busResponse = (Response<object>)await InvokeAsync(genericMethod, null, new object[] { massTransitBus, request, cancellationToken, default(RequestTimeout), null });
			return busResponse.Message;
		}

		public async Task SendAsync(Type requestType, object request, CancellationToken cancellationToken)
		{
			//await _massTransitBus.Publish(request, requestType, cancellationToken); //Wont get response
			//await _massTransitBus.Send(request, requestType, cancellationToken); //Does not work

			//Command can return response, but should not query data, returning command completion status is allowed
			await RequestAsync(requestType, request, typeof(VoidCommandResult), cancellationToken);
		}

		public async Task SendAsync(Type requestType, CancellationToken cancellationToken)
		{
			var request = Activator.CreateInstance(requestType);
			//await _massTransitBus.Publish(request, requestType, cancellationToken);
			await SendAsync(requestType, request, cancellationToken);
		}

		async Task ITypedMessageBusClient.SendAsync<TRequest>(CancellationToken cancellationToken)
		{
			//await _massTransitBus.Publish<TRequest>(cancellationToken);
			await SendAsync(typeof(TRequest), cancellationToken);
		}

		async Task ITypedMessageBusClient.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
		{
			//await _massTransitBus.Publish(request, cancellationToken);
			await SendAsync(typeof(TRequest), request, cancellationToken);
		}

		private static async Task<object> InvokeAsync(MethodInfo masstransitRequestMethod, object obj, params object[] parameters)
		{
			var task = (Task)masstransitRequestMethod.Invoke(obj, parameters);
			await task.ConfigureAwait(false);
			var resultProperty = task.GetType().GetProperty(nameof(Task<object>.Result));
			return resultProperty.GetValue(task);
		}

		public void Dispose()
		{
			try
			{
				massTransitBus.Start(new TimeSpan(0, 0, 30));
			}
			catch (Exception ex)
			{
				throw new Exception("MassTransit timeout", ex);
			}
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}