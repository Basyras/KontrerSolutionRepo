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
	public class ProxyHttpReqeustHandler
	{
		private readonly IByteMessageBusClient messageBus;
		private readonly ISimpleToByteSerailizer serializer;
		private static readonly string proxyRequestSimpleDatatype = TypedToSimpleConverter.ConvertTypeToSimple<ProxyRequest>();
		private static readonly string proxyResponseSimpleDataType = TypedToSimpleConverter.ConvertTypeToSimple<ProxyResponse>();

		public ProxyHttpReqeustHandler(IByteMessageBusClient messageBus, ISimpleToByteSerailizer serializer)
		{
			this.messageBus = messageBus;
			this.serializer = serializer;
		}

		public async Task Handle(HttpContext context)
		{
			MemoryStream httpBodyMemoryStream = new MemoryStream();
			await context.Request.Body.CopyToAsync(httpBodyMemoryStream);
			var proxyRequestBytes = httpBodyMemoryStream.ToArray();
			var proxyRequestResult = serializer.Deserialize(proxyRequestBytes, proxyRequestSimpleDatatype);
			ProxyRequest proxyRequest = (ProxyRequest)proxyRequestResult.Value;

			if (proxyRequest.HasResponse)
			{
				var busRequestResponse = await messageBus.RequestAsync(proxyRequest.MessageType, proxyRequest.MessageData);

				await busRequestResponse.Match(
					async responseBytes =>
					{
						var proxyResponse = new ProxyResponse(proxyRequest.MessageType, proxyRequest.HasResponse, responseBytes, "clientHasToDetermine");
						var proxyResponseSerializationResult = serializer.Serialize(proxyResponse, proxyResponseSimpleDataType);
						await proxyResponseSerializationResult.Match(
							async proxyResponseBytes =>
							{
								await context.Response.BodyWriter.WriteAsync(proxyResponseBytes);
							},
							proxyResponseSeriFailure => throw new Exception(proxyResponseSeriFailure.Message));
					},
					busRequestError => throw new Exception(busRequestError.Message));

			}
			else
			{
				await messageBus.SendAsync(proxyRequest.MessageType, proxyRequest.MessageData);
			}
		}
	}
}