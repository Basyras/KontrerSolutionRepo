using Kontrer.Shared.MessageBus.Proxy.Shared;
using Kontrer.Shared.MessageBus.RequestResponse;
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

        Task IMessageBusManager.SendAsync<TRequest>(CancellationToken cancellationToken)
        {
            return SendToProxy(new TRequest());
        }

        Task IMessageBusManager.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            return SendToProxy(request);
        }

        Task IMessageBusManager.SendAsync(Type requestType, object request, CancellationToken cancellationToken)
        {
            return SendToProxy(requestType, request);
        }

        Task IMessageBusManager.SendAsync(Type requestType, CancellationToken cancellationToken)
        {
            return SendToProxy(requestType);
        }

        Task<TResponse> IMessageBusManager.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
        {
            return RequestToProxy<TRequest, TResponse>(new TRequest());
        }

        Task<TResponse> IMessageBusManager.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            return RequestToProxy<TRequest, TResponse>(request);
        }

        Task<object> IMessageBusManager.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
        {
            return RequestToProxy(requestType, responseType, null);
        }

        Task<object> IMessageBusManager.RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken)
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

        private async Task SendToProxy(Type requestType, object request)
        {
            await HttpCallToProxyServer(requestType, request);
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
            var responseTypeName = responseType == null ? null : responseType.AssemblyQualifiedName;
            var proxyRequest = new ProxyRequest(requestType.AssemblyQualifiedName, responseTypeName, requestJson);
            var proxyRequestJson = serializer.Serialize(proxyRequest);
            var httpContent = new StringContent(proxyRequestJson, Encoding.UTF8, "application/json");
            var httpResult = await httpClient.PostAsync("", httpContent);
            if (responseType == null)
            {
                return null;
            }
            MemoryStream mem = new MemoryStream();
            await httpResult.Content.CopyToAsync(mem);
            var bytes = mem.ToArray();
            var busResponse = serializer.Deserialize(bytes, responseType);
            //string resultContent = await httpResult.Content.ReadAsStringAsync();
            return busResponse;
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