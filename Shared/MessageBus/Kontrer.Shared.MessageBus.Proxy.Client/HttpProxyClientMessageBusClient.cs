using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client
{
    public class HttpProxyClientMessageBusClient : ITypedMessageBusClient
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<MessageBusHttpProxyClientOptions> options;
        private readonly IRequestSerializer serializer;

        public HttpProxyClientMessageBusClient(IOptions<MessageBusHttpProxyClientOptions> options, /*HttpClient httpClient,*/ IRequestSerializer serializer)
        {
            this.httpClient = new HttpClient() { BaseAddress = options.Value.ProxyHostUri };
            this.options = options;
            this.serializer = serializer;
        }

        Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
        {
            return SendToProxy(new TEvent());
        }

        Task ITypedMessageBusClient.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
        {
            return SendToProxy(data);
        }

        Task ITypedMessageBusClient.SendAsync<TRequest>(CancellationToken cancellationToken)
        {
            return SendToProxy(new TRequest());
        }

        Task ITypedMessageBusClient.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            return SendToProxy(request);
        }

        public Task SendAsync(Type requestType, object request, CancellationToken cancellationToken)
        {
            return SendToProxy(requestType, request);
        }

        Task ITypedMessageBusClient.SendAsync(Type requestType, CancellationToken cancellationToken)
        {
            return SendToProxy(requestType);
        }

        Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
        {
            return RequestToProxy<TRequest, TResponse>(new TRequest());
        }

        Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            return RequestToProxy<TRequest, TResponse>(request);
        }

        Task<object> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
        {
            return RequestToProxy(requestType, responseType, null);
        }

        public Task<object> RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken)
        {
            return RequestToProxy(requestType, responseType, request);
        }

        private Task SendToProxy<TRequest>(TRequest request)
        {
            return SendToProxy(typeof(TRequest), request);
        }

        private Task SendToProxy(Type requestType)
        {
            return SendToProxy(requestType, Activator.CreateInstance(requestType));
        }

        private Task SendToProxy(Type requestType, object request)
        {
            return HttpCallToProxyServer(requestType, request);
        }

        private Task<object> RequestToProxy(Type requestType, Type responseType, object request = null)
        {
            request = request ?? Activator.CreateInstance(requestType);
            var result = HttpCallToProxyServer(requestType, request, responseType);
            return result;
        }

        private async Task<TResponse> RequestToProxy<TRequest, TResponse>(TRequest request)
        {
            var result = await HttpCallToProxyServer(typeof(TRequest), request, typeof(TResponse));
            return (TResponse)result;
        }

        private async Task<object> HttpCallToProxyServer(Type requestType, object request, Type responseType = null)
        {
            var requestJson = serializer.Serialize(request, requestType);
            var proxyRequest = ProxyRequest.Create(requestType, requestJson, responseType);
            var proxyRequestJson = serializer.Serialize(proxyRequest);
            var httpContent = new StringContent(proxyRequestJson, Encoding.UTF8, "application/json");
            var httpResult = await httpClient.PostAsync("", httpContent);

            if (httpResult.IsSuccessStatusCode is false)
            {
                var httpErrorContent = await httpResult.Content.ReadAsStringAsync();
                throw new Exception($"Message bus response failure, code: {(int)httpResult.StatusCode},\nreason: {httpResult.ReasonPhrase},\ncontent: {httpErrorContent}");
            }

            if (responseType == null)
            {
                return null;
            }

            MemoryStream httpMemomoryStream = new MemoryStream();
            await httpResult.Content.CopyToAsync(httpMemomoryStream);
            var bytes = httpMemomoryStream.ToArray();
            var busResponse = serializer.Deserialize(bytes, responseType);
            return busResponse;
        }

        public void Dispose()
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}