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
		private static readonly string proxyRequestSimpleType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ProxyResponse));


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
			var proxyRequest = (ProxyRequest)serializer.Deserialize(httpBodyBytes, proxyRequestSimpleDatatype);
			var requestData = serializer.Deserialize(proxyRequest.MessageBytes, proxyRequest.MessageType);

			if (proxyRequest.HasResponse)
			{
				var busRequestResponse = await messageBus.RequestAsync(proxyRequest.MessageType, requestData);
				await busRequestResponse.Match(
					async response =>
					{
						var responseType = TypedToSimpleConverter.ConvertTypeToSimple(response.GetType());
						var responseBytes = serializer.Serialize(response, responseType);
						var proxyResponse = new ProxyResponse(proxyRequest.MessageType, proxyRequest.HasResponse, responseBytes, responseType);
						var proxyResponseBytes = serializer.Serialize(proxyResponse, proxyRequestSimpleType);
						await context.Response.BodyWriter.WriteAsync(proxyResponseBytes);
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