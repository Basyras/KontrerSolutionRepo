using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.Options;
using OneOf;
using Polly;
using Polly.Wrap;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client
{
	public class HttpProxyObjectMessageBusClient : IObjectMessageBusClient
	{
		private static readonly string proxyResponseSimpleDataType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyResponse));
		private readonly AsyncPolicyWrap retryPolicy;
		private readonly HttpClient httpClient;
		private readonly IOptions<HttpProxyObjectMessageBusClientOptions> options;
		private readonly IObjectToByteSerailizer objectToByteSerializer;
		private readonly string wrapperMessageType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyRequest));


		public HttpProxyObjectMessageBusClient(IOptions<HttpProxyObjectMessageBusClientOptions> options,
			IObjectToByteSerailizer byteSerializer)
		{
			retryPolicy = Policy.Handle<Exception>().RetryAsync(2).WrapAsync(Policy.TimeoutAsync(2, Polly.Timeout.TimeoutStrategy.Pessimistic));
			this.httpClient = new HttpClient() { BaseAddress = options.Value.ProxyHostUri };
			this.options = options;
			this.objectToByteSerializer = byteSerializer;
		}

		private async Task<object> HttpCallToProxyServer(string messageType, object messageData, Type responseType = null, CancellationToken cancellationToken = default)
		{
			if (objectToByteSerializer.TrySerialize(messageData, messageType, out var requestBytes, out var seriError) is false)
			{
				return seriError;
			}

			var responseTypeString = responseType?.AssemblyQualifiedName;
			var hasResponse = responseType != null;
			var proxyRequest = new ProxyRequest(messageType, hasResponse, requestBytes, responseTypeString);

			if (objectToByteSerializer.TrySerialize(proxyRequest, wrapperMessageType, out var proxyRequestBytes, out var error) is false)
			{
				return error;
			}

			var httpContent = new ByteArrayContent(proxyRequestBytes);
			//var httpResult = await httpClient.PostAsync("", httpContent);
			var httpResult = await retryPolicy.ExecuteAsync(async () => await httpClient.PostAsync("", httpContent));

			if (httpResult.IsSuccessStatusCode is false)
			{
				var httpErrorContent = await httpResult.Content.ReadAsStringAsync();
				throw new Exception($"Message bus response failure, code: {(int)httpResult.StatusCode},\nreason: {httpResult.ReasonPhrase},\ncontent: {httpErrorContent}");
			}

			if (hasResponse is false)
				return null;

			cancellationToken.ThrowIfCancellationRequested();

			using MemoryStream httpMemomoryStream = new MemoryStream();
			await httpResult.Content.CopyToAsync(httpMemomoryStream);
			var proxyResponseResponseBytes = httpMemomoryStream.ToArray();

			cancellationToken.ThrowIfCancellationRequested();

			var proxyResponse = (ProxyResponse)objectToByteSerializer.Deserialize(proxyResponseResponseBytes, proxyResponseSimpleDataType);
			return objectToByteSerializer.Deserialize(proxyResponse.ResponseBytes, proxyResponse.ResponseType);
		}

		public void Dispose()
		{

		}

		Task IObjectMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(eventType, null, null, cancellationToken);
		}

		Task IObjectMessageBusClient.PublishAsync(string eventType, object eventData, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(eventType, eventData, null, cancellationToken);
		}

		Task IObjectMessageBusClient.SendAsync(string commandType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(commandType, null, null, cancellationToken);
		}

		Task IObjectMessageBusClient.SendAsync(string commandType, object commandData, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(commandType, commandData, null, cancellationToken);
		}

		Task<object> IObjectMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(requestType, null, typeof(UknownResponseType), cancellationToken);
		}

		async Task<OneOf<object, ErrorMessage>> IObjectMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
		{
			var result = await HttpCallToProxyServer(requestType, requestData, typeof(UknownResponseType), cancellationToken);
			return result;
		}

		Task IObjectMessageBusClient.StartAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		void IDisposable.Dispose()
		{

		}

		private class UknownResponseType { };
	}
}