using Kontrer.Shared.MessageBus.RequestResponse;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Client
{
    public class ProxyMessageBusManager : IMessageBusManager
    {
        private readonly HttpClient httpClient;

        public ProxyMessageBusManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public ProxyMessageBusManager(Uri busProxy)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = busProxy;
        }

        Task IMessageBusManager.PublishAsync<TEvent>(CancellationToken cancellationToken)
        {
            return SendToProxy<TEvent>(new TEvent());
        }

        Task IMessageBusManager.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
        {
            return SendToProxy<TEvent>(data);
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
            return SendToProxy<TRequest>(new TRequest());
        }

        Task IMessageBusManager.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            return SendToProxy<TRequest>(request);
        }

        private async Task<TResponse> SendToProxy<TRequest, TResponse>(TRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("", content);
            string resultContent = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(resultContent);
        }

        private async Task SendToProxy<TRequest>(TRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("", content);
            string resultContent = await result.Content.ReadAsStringAsync();
        }
    }
}