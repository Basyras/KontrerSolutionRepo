using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using Basyc.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Server.Asp
{
    public class ProxyHttpReqeustHandler
    {
        private readonly ISimpleMessageBusClient messageBus;
        private readonly ISimpleByteSerailizer serializer;

        public ProxyHttpReqeustHandler(ISimpleMessageBusClient messageBus, ISimpleByteSerailizer serializer)
        {
            this.messageBus = messageBus;
            this.serializer = serializer;
        }

        public async Task Handle(HttpContext context)
        {            
            MemoryStream mem = new MemoryStream();
            await context.Request.Body.CopyToAsync(mem);
            var bytes = mem.ToArray();
            var proxyRequestResult = serializer.Deserialize(bytes, TypedToSimpleConverter.ConvertTypeToSimple<ProxyRequest>());
            ProxyRequest proxyRequest = (ProxyRequest)proxyRequestResult.Value;

            var requestDeserializatioResult = serializer.Deserialize(proxyRequest.RequestData, proxyRequest.MessageType);
            if(requestDeserializatioResult.Value is SerializationFailure)
            {
                throw new Exception(requestDeserializatioResult.AsT1.Message);
            }
            var requestData = requestDeserializatioResult.AsT0;

            //if (requestType.IsAssignableTo(typeof(IMessage)))
            //{
            //    await messageBus.SendAsync(requestType, requestResult);
            //    return;
            //}

            //if (GenericsHelper.IsAssignableToGenericType(requestType, typeof(IMessage<>)))
            //{
            //    var responseType = GenericsHelper.GetTypeArgumentsFromParent(requestType, typeof(IMessage<>))[0];
            //    var busResponse = await messageBus.RequestAsync(requestType, request, responseType);
            //    await context.Response.WriteAsync(serializer.Serialize(busResponse, responseType));
            //    return;
            //}


            //await messageBus.SendAsync(proxyRequest.MessageType, requestData);

            //var requestType = TypedToSimpleConverter.ConvertSimpleToType(proxyRequest.MessageType);

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
                            async bytes => await context.Response.BodyWriter.WriteAsync(bytes),
                            failure => throw new Exception(responseSerializationResult.AsT1.Message));
                    },
                    error => throw new Exception(error.Message));

            }
            else
            {
                await messageBus.SendAsync(proxyRequest.MessageType, requestData);
            }
        }
    }
}