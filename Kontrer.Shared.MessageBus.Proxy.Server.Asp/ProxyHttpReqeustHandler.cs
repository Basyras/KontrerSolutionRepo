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
            //var requestType = Type.GetType(proxyRequest.MessageType);

            //if (requestType == null)
            //    throw new Exception("Request type is not loaded or does not exist");

            //var request = serializer.Deserialize(proxyRequest.RequestData, requestType);
            //if (request == null) //Messages with 0 parameters can be just created
            //{
            //    request = Activator.CreateInstance(requestType);
            //}

            var requestResult = serializer.Deserialize(proxyRequest.RequestData, proxyRequest.MessageType);
            if(requestResult.Value is SerializationFailure)
            {
                throw new Exception(requestResult.AsT1.Message);
            }
            var requestData = requestResult.AsT0;


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

             await messageBus.SendAsync(proxyRequest.MessageType, requestData);

            //throw new InvalidOperationException($"IMessage does not inherit from {nameof(IMessage)}");
        }
    }
}