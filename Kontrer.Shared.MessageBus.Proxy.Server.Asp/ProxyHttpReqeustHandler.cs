using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.HttpProxy.Server.Asp
{
    public class ProxyHttpReqeustHandler
    {
        private readonly IMessageBusManager messageBus;
        private readonly IRequestSerializer serializer;

        public ProxyHttpReqeustHandler(IMessageBusManager messageBus, IRequestSerializer serializer)
        {
            this.messageBus = messageBus;
            this.serializer = serializer;
        }

        public async Task Handle(HttpContext context)
        {
            ProxyRequest proxyRequest = await ParseProxyRequestFromHttp(context);
            var requestType = Type.GetType(proxyRequest.RequestAssemblyQualifiedTypeName);

            if (requestType == null)
                throw new Exception("Request type is not loaded or does not exist");

            var request = serializer.Deserialize(proxyRequest.RequestJson, requestType);
            if (request == null) //Messages with 0 parameters can be just created
            {
                request = Activator.CreateInstance(requestType);
            }

            if (requestType.IsAssignableTo(typeof(IRequest)))
            {
                await messageBus.SendAsync(requestType, request);
                return;
            }

            if (GenericsHelper.IsAssignableToGenericType(requestType, typeof(IRequest<>)))
            {
                var responseType = GenericsHelper.GetTypeArgumentsFromParent(requestType, typeof(IRequest<>))[0];
                var busResponse = await messageBus.RequestAsync(requestType, request, responseType);
                await context.Response.WriteAsync(serializer.Serialize(busResponse, responseType));
                return;
            }

            throw new InvalidOperationException($"Request does not inherit from {nameof(IRequest)}");
        }

        private async Task<ProxyRequest> ParseProxyRequestFromHttp(HttpContext context)
        {
            MemoryStream mem = new MemoryStream();
            await context.Request.Body.CopyToAsync(mem);
            var bytes = mem.ToArray();
            var proxyRequest = serializer.Deserialize<ProxyRequest>(bytes);
            return proxyRequest;
        }
    }
}