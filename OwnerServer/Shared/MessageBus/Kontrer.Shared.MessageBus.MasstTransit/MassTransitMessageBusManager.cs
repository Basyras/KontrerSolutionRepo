using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kontrer.Shared.MessageBus.PublishSubscribe;
using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.Shared.MessageBus;
using System.Reflection;

namespace Kontrer.Shared.MessageBus.MasstTransit
{
    public class MassTransitMessageBusManager : IMessageBusManager
    {
        private readonly IBusControl _massTransitBus;

        public MassTransitMessageBusManager(IBusControl massTransitBus)
        {
            _massTransitBus = massTransitBus;
        }

        Task IMessageBusManager.PublishAsync<TEvent>(CancellationToken cancellationToken)
        {
            return _massTransitBus.Publish<TEvent>(cancellationToken);
        }

        Task IMessageBusManager.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
        {
            return _massTransitBus.Publish(data, cancellationToken);
        }

        async Task<TResponse> IMessageBusManager.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
        {
            var response = await _massTransitBus.Request<TRequest, TResponse>(cancellationToken);
            return response.Message;
        }

        async Task<TResponse> IMessageBusManager.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            var response = await _massTransitBus.Request<TRequest, TResponse>(request, cancellationToken);
            return response.Message;
        }

        async Task<object> IMessageBusManager.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
        {
            var bus = (IMessageBusManager)this;
            return await bus.RequestAsync(requestType, Activator.CreateInstance(requestType), responseType, cancellationToken);
        }

        async Task<object> IMessageBusManager.RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken)
        {
            var conType = typeof(SendContext<>).MakeGenericType(requestType);
            var actionType = typeof(Action<>).MakeGenericType(conType);
            var methodParameterTypes = new Type[] { typeof(IBus), requestType, typeof(CancellationToken), typeof(RequestTimeout), actionType };
            //MethodInfo requestMethodInfo = typeof(MassTransit.RequestExtensions).GetMethod(nameof(MassTransit.RequestExtensions.Request), BindingFlags.Public | BindingFlags.Static, null, methodParameterTypes, null);
            MethodInfo requestMethodInfo = typeof(MassTransit.RequestExtensions).GetMethods().Where(x => x.Name == nameof(MassTransit.RequestExtensions.Request)).Skip(2).First();
            var parameters = requestMethodInfo.GetParameters();
            MethodInfo genericMethod = requestMethodInfo.MakeGenericMethod(requestType, responseType);

            var busResponse = (Response<object>)await InvokeAsync(genericMethod, null, new object[] { _massTransitBus, request, cancellationToken, default(RequestTimeout), null });
            return busResponse.Message;
        }

        async Task IMessageBusManager.SendAsync(Type requestType, object request, CancellationToken cancellationToken)
        {
            await _massTransitBus.Publish(request, requestType, cancellationToken);
        }

        async Task IMessageBusManager.SendAsync(Type requestType, CancellationToken cancellationToken)
        {
            var request = Activator.CreateInstance(requestType);
            await _massTransitBus.Publish(request, requestType, cancellationToken);
        }

        async Task IMessageBusManager.SendAsync<TRequest>(CancellationToken cancellationToken)
        {
            await _massTransitBus.Publish<TRequest>(cancellationToken);
        }

        async Task IMessageBusManager.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            await _massTransitBus.Publish(request, cancellationToken);
        }

        private static async Task<object> InvokeAsync(MethodInfo @this, object obj, params object[] parameters)
        {
            var task = (Task)@this.Invoke(obj, parameters);
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }
    }
}