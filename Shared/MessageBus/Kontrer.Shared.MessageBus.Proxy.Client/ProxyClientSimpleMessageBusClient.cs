using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.Options;
using OneOf;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client
{
	public class ProxyClientSimpleMessageBusClient : ISimpleMessageBusClient
	{
		private readonly HttpClient httpClient;
		private readonly IOptions<MessageBusHttpProxyClientOptions> options;
		private readonly ISimpleToByteSerailizer byteSerializer;
		private readonly string wrapperMessageType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyRequest));
		private static readonly string proxyResponseSimpleDataType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyResponse));


		public ProxyClientSimpleMessageBusClient(IOptions<MessageBusHttpProxyClientOptions> options,
			ISimpleToByteSerailizer byteSerializer)
		{
			this.httpClient = new HttpClient() { BaseAddress = options.Value.ProxyHostUri };
			this.options = options;
			this.byteSerializer = byteSerializer;
		}

		private async Task<object> HttpCallToProxyServer(string messageType, object messageData, Type responseType = null, CancellationToken cancellationToken = default)
		{
			var seriResult = byteSerializer.Serialize(messageData, messageType);
			if (seriResult.IsT1)
				return seriResult.AsT1;

			var responseTypeString = responseType?.AssemblyQualifiedName;
			var hasResponse = responseType != null;
			var proxyRequest = new ProxyRequest(messageType, hasResponse, seriResult.AsT0, responseTypeString);

			var proxyRequestSerializationResult = byteSerializer.Serialize(proxyRequest, wrapperMessageType);

			if (proxyRequestSerializationResult.IsT1)
				return proxyRequestSerializationResult.AsT1;

			var proxyRequestBytes = proxyRequestSerializationResult.AsT0;
			var httpContent = new ByteArrayContent(proxyRequestBytes);
			var httpResult = await httpClient.PostAsync("", httpContent);

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

			var proxyResponseSeriResult = byteSerializer.Deserialize(proxyResponseResponseBytes, proxyResponseSimpleDataType);
			//return proxyResponseSeriResult.Match(
			//	proxyResponseObject =>
			//	{
			//		var proxyResponse = (ProxyResponse)proxyResponseObject;
			//		var responseSeriResult = byteSerializer.Deserialize(proxyResponse.ResponseBytes, proxyResponse.ResponseType);
			//		return responseSeriResult.Match(
			//			responseObject => responseObject,
			//			responseSeriFailure => throw new Exception(responseSeriFailure.Message));
			//	},
			//	proxyResponseSeriFailure => throw new Exception(proxyResponseSeriFailure.Message));

			var proxyResponse = (ProxyResponse)proxyResponseSeriResult.AsT0;
			if (proxyResponse.ResponseType == "clientHasToDetermine")
			{
				return proxyResponse.ResponseBytes;
			}
			else
			{

			}

			var responseSeriResult = byteSerializer.Deserialize(proxyResponse.ResponseBytes, proxyResponse.ResponseType);
			return responseSeriResult.Match(
				responseObject => responseObject,
				responseSeriFailure => throw new Exception(responseSeriFailure.Message));
		}

		public void Dispose()
		{

		}

		Task ISimpleMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(eventType, null, null, cancellationToken);
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
			return HttpCallToProxyServer(requestType, null, typeof(UknownResponseType), cancellationToken);
		}

		async Task<OneOf<object, ErrorMessage>> ISimpleMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
		{
			//return (OneOf<object, ErrorMessage>)await HttpCallToProxyServer(requestType, requestData, typeof(UknownResponseType), cancellationToken);
			var result = await HttpCallToProxyServer(requestType, requestData, typeof(UknownResponseType), cancellationToken);
			return result;
		}

		Task ISimpleMessageBusClient.StartAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		void IDisposable.Dispose()
		{

		}

		private class UknownResponseType { };
	}
}