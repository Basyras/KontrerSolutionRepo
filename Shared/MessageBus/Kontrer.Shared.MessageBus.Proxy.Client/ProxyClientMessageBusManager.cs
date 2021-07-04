using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.Shared.MessageBux.Proxy.Shared;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Proxy.Client
{
    public class ProxyClientMessageBusManager : IMessageBusManager
    {
        private readonly HttpClient httpClient;
        private readonly IRequestSerializer serializer;

        public ProxyClientMessageBusManager(Uri busProxy, IRequestSerializer serializer) : this(new HttpClient() { BaseAddress = busProxy }, serializer)
        {
        }

        public ProxyClientMessageBusManager(HttpClient httpClient, IRequestSerializer serializer)
        {
            this.httpClient = httpClient;
            this.serializer = serializer;
        }

        Task IMessageBusManager.PublishAsync<TEvent>(CancellationToken cancellationToken)
        {
            return SendToProxy(new TEvent());
        }

        Task IMessageBusManager.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
        {
            return SendToProxy(data);
        }

        Task<TResponse> IMessageBusManager.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
        {
            return SendToProxy<TRequest, TResponse>(new TRequest());
        }

        Task<TResponse> IMessageBusManager.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            return SendToProxy<TRequest, TResponse>(request);
        }

        Task IMessageBusManager.SendAsync<TRequest>(CancellationToken cancellationToken)
        {
            return SendToProxy(new TRequest());
        }

        Task IMessageBusManager.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            return SendToProxy(request);
        }

        private async Task<TResponse> SendToProxy<TRequest, TResponse>(TRequest request)
        {
            var json = serializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("", content);
            string resultContent = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(resultContent);
        }

        private async Task SendToProxy<TRequest>(TRequest request)
        {
            var json = serializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("", content);
            string resultContent = await result.Content.ReadAsStringAsync();
        }
    }
}