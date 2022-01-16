using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
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
        private readonly ISimpleByteSerailizer byteSerializer;
        private readonly string wrapperMessageType;

        public HttpProxyClientMessageBusClient(IOptions<MessageBusHttpProxyClientOptions> options, 
            ISimpleByteSerailizer byteSerializer)
        {
            this.httpClient = new HttpClient() { BaseAddress = options.Value.ProxyHostUri };
            this.options = options;
            this.byteSerializer = byteSerializer;
            wrapperMessageType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyRequest));
        }

        private async Task<object> HttpCallToProxyServer(string messageType, object messageData, Type responseType = null, CancellationToken cancellationToken = default)
        {            
            var seriResult = byteSerializer.Serialize(messageData, messageType);
            if (seriResult.IsT1)
                return seriResult.AsT1;

            var responseTypeString = responseType?.AssemblyQualifiedName;
            var proxyRequest = new ProxyRequest(messageType, seriResult.AsT0, responseTypeString);
            
            var proxySeriResult = byteSerializer.Serialize(proxyRequest, wrapperMessageType);
            if (proxySeriResult.IsT1)
                return proxySeriResult.AsT1;

            var proxyBytes = proxySeriResult.AsT0;
            var httpContent = new ByteArrayContent(proxyBytes);
            var httpResult = await httpClient.PostAsync("", httpContent);

            if (httpResult.IsSuccessStatusCode is false)
            {
                var httpErrorContent = await httpResult.Content.ReadAsStringAsync();
                throw new Exception($"Message bus response failure, code: {(int)httpResult.StatusCode},\nreason: {httpResult.ReasonPhrase},\ncontent: {httpErrorContent}");
            }

            if (responseType == null)
                return null;

            cancellationToken.ThrowIfCancellationRequested();

            using MemoryStream httpMemomoryStream = new MemoryStream();
            await httpResult.Content.CopyToAsync(httpMemomoryStream);
            var bytes = httpMemomoryStream.ToArray();
            cancellationToken.ThrowIfCancellationRequested();

            var busResponse = byteSerializer.Deserialize(bytes, TypedToSimpleConverter.ConvertTypeToSimple(responseType));
            if(busResponse.Value is SerializationFailure failure)
                throw new Exception(failure.Message);

            cancellationToken.ThrowIfCancellationRequested();

            return busResponse.AsT0;
        }

        public void Dispose()
        {
            
        }

        Task ISimpleMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
        {
            return HttpCallToProxyServer(eventType,null,null, cancellationToken);
        }

        Task ISimpleMessageBusClient.PublishAsync(string eventType, object eventData, CancellationToken cancellationToken)
        {
            return HttpCallToProxyServer(eventType, eventData, null, cancellationToken);
        }

        Task ISimpleMessageBusClient.SendAsync(string commandType, CancellationToken cancellationToken)
        {
            return HttpCallToProxyServer(commandType, null, null, cancellationToken);
        }

        Task ISimpleMessageBusClient.SendAsync(string commandType, object commandData, CancellationToken cancellationToken)
        {
            return HttpCallToProxyServer(commandType, commandData, null, cancellationToken);
        }

        Task<object> ISimpleMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
        {
            return HttpCallToProxyServer(requestType,null, typeof(object), cancellationToken);
        }

        async Task<OneOf<object, ErrorMessage>> ISimpleMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
        {
            return (OneOf<object, ErrorMessage>)await HttpCallToProxyServer(requestType, requestData, typeof(object), cancellationToken);
        }

        Task ISimpleMessageBusClient.StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            
        }
    }
}