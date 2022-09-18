using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Server.Asp.Http
{
	public class ProxyHttpRequestHandler
	{
		private readonly IByteMessageBusClient messageBus;
		private readonly IObjectToByteSerailizer serializer;
		private static readonly string proxyRequestSimpleDatatype = TypedToSimpleConverter.ConvertTypeToSimple<ProxyRequest>();
		private static readonly string proxyResponseSimpleDataType = TypedToSimpleConverter.ConvertTypeToSimple<ProxyResponse>();

		public ProxyHttpRequestHandler(IByteMessageBusClient messageBus, IObjectToByteSerailizer serializer)
		{
			this.messageBus = messageBus;
			this.serializer = serializer;
		}

		public async Task Handle(HttpContext context)
		{
			MemoryStream httpBodyMemoryStream = new MemoryStream();
			await context.Request.Body.CopyToAsync(httpBodyMemoryStream);
			var proxyRequestBytes = httpBodyMemoryStream.ToArray();
			ProxyRequest proxyRequest = (ProxyRequest)serializer.Deserialize(proxyRequestBytes, proxyRequestSimpleDatatype);

			if (proxyRequest.HasResponse)
			{
				var busTask = messageBus.RequestAsync(proxyRequest.MessageType, proxyRequest.MessageBytes);
				var busTaskValue = await busTask.Task;
				await busTaskValue.Match(
					async byteResponse =>
					{
						var proxyResponse = new ProxyResponse(proxyRequest.MessageType, proxyRequest.HasResponse, byteResponse.ResponseBytes, byteResponse.ResposneType);
						var proxyResponseBytes = serializer.Serialize(proxyResponse, proxyResponseSimpleDataType);
						await context.Response.BodyWriter.WriteAsync(proxyResponseBytes);
					},
					busRequestError => throw new Exception(busRequestError.Message));
			}
			else
			{
				await messageBus.SendAsync(proxyRequest.MessageType, proxyRequest.MessageBytes);
			}
		}
	}
}