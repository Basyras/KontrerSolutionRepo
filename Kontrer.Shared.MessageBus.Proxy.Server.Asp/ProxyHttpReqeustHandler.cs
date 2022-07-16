using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Server.Asp
{
	public class ProxyHttpReqeustHandler2
	{
		private readonly IObjectMessageBusClient messageBus;
		private readonly IObjectToByteSerailizer serializer;
		private static readonly string proxyRequestSimpleDatatype = TypedToSimpleConverter.ConvertTypeToSimple<ProxyRequest>();

		public ProxyHttpReqeustHandler2(IObjectMessageBusClient messageBus, IObjectToByteSerailizer serializer)
		{
			this.messageBus = messageBus;
			this.serializer = serializer;
		}

		public async Task Handle(HttpContext context)
		{
			MemoryStream httpBodyMemoryStream = new MemoryStream();
			await context.Request.Body.CopyToAsync(httpBodyMemoryStream);
			var httpBodyBytes = httpBodyMemoryStream.ToArray();
			var proxyRequestResult = serializer.Deserialize(httpBodyBytes, proxyRequestSimpleDatatype);
			ProxyRequest proxyRequest = (ProxyRequest)proxyRequestResult.Value;

			var requestDeserializatioResult = serializer.Deserialize(proxyRequest.MessageData, proxyRequest.MessageType);
			if (requestDeserializatioResult.Value is SerializationFailure)
			{
				throw new Exception(requestDeserializatioResult.AsT1.Message);
			}
			var requestData = requestDeserializatioResult.AsT0;

			if (proxyRequest.HasResponse)
			{
				//var responseType = GenericsHelper.GetTypeArgumentsFromParent(requestType, typeof(IMessage<>))[0];
				var busRequestResponse = await messageBus.RequestAsync(proxyRequest.MessageType, requestData);
				//var responseSerializationResult = serializer.Serialize(busRequestResponse, busRequestResponse.AsT0);
				//await responseSerializationResult.Match(
				//    async bytes => await context.Response.BodyWriter.WriteAsync(bytes),
				//    failure => throw new Exception(responseSerializationResult.AsT1.Message));

				await busRequestResponse.Match(
					async response =>
					{
						var responseType = TypedToSimpleConverter.ConvertTypeToSimple(response.GetType());
						var responseSerializationResult = serializer.Serialize(response, responseType);
						await responseSerializationResult.Match(
							async responseBytes =>
							{
								var proxyResponse = new ProxyResponse(proxyRequest.MessageType, proxyRequest.HasResponse, responseBytes, responseType);
								var proxyResponseSerializationResult = serializer.Serialize(proxyResponse, TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyResponse)));
								await proxyResponseSerializationResult.Match(
									async proxyResponseBytes =>
									{
										await context.Response.BodyWriter.WriteAsync(proxyResponseBytes);
									},
									proxyResponseSeriFailure => throw new Exception(proxyResponseSerializationResult.AsT1.Message));
							},
							responseSeriFailure => throw new Exception(responseSerializationResult.AsT1.Message));
					},
					busRequestError => throw new Exception(busRequestError.Message));

			}
			else
			{
				await messageBus.SendAsync(proxyRequest.MessageType, requestData);
			}
		}
	}
}