using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Microsoft.Extensions.Options;
using OneOf;
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
    public class HttpProxyClientMessageBusClient : ISimpleMessageBusClient
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<MessageBusHttpProxyClientOptions> options;
        private readonly IRequestSerializer serializer;
        private readonly ISimpleByteSerailizer byteSerializer;

        public HttpProxyClientMessageBusClient(IOptions<MessageBusHttpProxyClientOptions> options, 
            /*HttpClient httpClient,*/ 
            IRequestSerializer serializer,
            ISimpleByteSerailizer byteSerializer)
        {
            this.httpClient = new HttpClient() { BaseAddress = options.Value.ProxyHostUri };
            this.options = options;
            this.serializer = serializer;
            this.byteSerializer = byteSerializer;
        }

        //Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
        //{
        //    return SendToProxy(new TEvent());
        //}

        //Task ITypedMessageBusClient.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
        //{
        //    return SendToProxy(data);
        //}

        //Task ITypedMessageBusClient.SendAsync<TRequest>(CancellationToken cancellationToken)
        //{
        //    return SendToProxy(new TRequest());
        //}

        //Task ITypedMessageBusClient.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        //{
        //    return SendToProxy(request);
        //}

        //public Task SendAsync(Type requestType, object request, CancellationToken cancellationToken)
        //{
        //    return SendToProxy(requestType, request);
        //}

        //Task ITypedMessageBusClient.SendAsync(Type requestType, CancellationToken cancellationToken)
        //{
        //    return SendToProxy(requestType);
        //}

        //async Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
        //{
        //    //return RequestToProxy<TRequest, TResponse>(new TRequest());
        //    return (TResponse) await RequestToProxy(typeof(TRequest),typeof(TResponse),new TRequest());
        //}

        //Task<OneOf<TResponse, ErrorMessage>> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        //{

        //}

        //Task<object> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
        //{
        //    return RequestToProxy(requestType, responseType, null);
        //}

        public Task<object> RequestAsync(string requestType, object requestData, Type responseType, CancellationToken cancellationToken)
        {
            return RequestToProxy(requestType, responseType, requestData);
        }

        //private Task SendToProxy<TRequest>(TRequest request)
        //{
        //    return SendToProxy(typeof(TRequest), request);
        //}

        //private Task SendToProxy(Type requestType)
        //{
        //    return SendToProxy(requestType, Activator.CreateInstance(requestType));
        //}

        //private Task SendToProxy(Type requestType, object request)
        //{
        //    return HttpCallToProxyServer(requestType, request);
        //}


        private Task<object> RequestToProxy(string requestType, Type responseType, object requestData = null)
        {
            //requestData = requestData ?? Activator.CreateInstance(requestType);
            var result = HttpCallToProxyServer(requestType, requestData, responseType);
            return result;
        }

        private async Task<object> HttpCallToProxyServer(string requestType, object reqeustData, Type responseType = null)
        {
            //var requestJson = serializer.Serialize(reqeustData, requestType);
            var seriResult = byteSerializer.Serialize(reqeustData, requestType);
            //var requestJson = serializer.Serialize(reqeustData, requestType);
            var seriResult2 = byteSerializer.Serialize(seriResult, requestType);
            var proxyRequest = ProxyRequest.Create(requestType, seriResult2.AsT0, responseType);
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

        Task ISimpleMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
        {
            return SendToProxy();
        }

        Task ISimpleMessageBusClient.PublishAsync(string eventType, object eventData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISimpleMessageBusClient.SendAsync(string commandType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISimpleMessageBusClient.SendAsync(string commandType, object commandData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<object> ISimpleMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<OneOf<object, ErrorMessage>> ISimpleMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISimpleMessageBusClient.StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}