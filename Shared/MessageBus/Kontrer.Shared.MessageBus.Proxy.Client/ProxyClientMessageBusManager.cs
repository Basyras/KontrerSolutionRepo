using Kontrer.Shared.MessageBus.Proxy.Shared;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Proxy.Client
{
    public class ProxyClientMessageBusManager : IMessageBusManager
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<MessageBusProxyClientOptions> options;
        private readonly IRequestSerializer serializer;

        public ProxyClientMessageBusManager(IOptions<MessageBusProxyClientOptions> options, IRequestSerializer serializer)
        {
            this.httpClient = new HttpClient() { BaseAddress = options.Value.ProxyHostUri };
            this.options = options;
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

        Task IMessageBusManager.SendAsync(Type requestType, object request = null, CancellationToken cancellationToken = default)
        {
            return SendToProxy(requestType, request);
        }

        private async Task<TResponse> SendToProxy<TRequest, TResponse>(TRequest request)
        {
            var json = serializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("", content);
            string resultContent = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(resultContent);
        }

        private Task SendToProxy<TRequest>(TRequest request)
        {
            return SendToProxy(request.GetType(), request);
        }

        private async Task SendToProxy(Type requestType, object request)
        {
            var proxyRequestJson = serializer.Serialize(new ProxyRequest(requestType.Name, serializer.Serialize(request, requestType)));
            var httpContent = new StringContent(proxyRequestJson, Encoding.UTF8, "application/json");
            var httpResult = await httpClient.PostAsync("", httpContent);
            string resultContent = await httpResult.Content.ReadAsStringAsync();
        }

        //private static Dictionary<string, object> DictionaryFromType(object atype)
        //{
        //    if (atype == null)
        //        return new Dictionary<string, object>();

        //    Type t = atype.GetType();
        //    PropertyInfo[] props = t.GetProperties();
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    foreach (PropertyInfo prp in props)
        //    {
        //        object value = prp.GetValue(atype, new object[] { });
        //        dict.Add(prp.Name, value);
        //    }
        //    return dict;
        //}
    }
}