using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IEnpointBuilderHttpProxyServerExtensions
    {
        public static void MapMessageBusProxyServer(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("", HandleRequest);
        }

        private static async Task HandleRequest(HttpContext context)
        {
            IMessageBusManager messageBus = context.RequestServices.GetRequiredService<IMessageBusManager>();
            var serializer = context.RequestServices.GetRequiredService<IRequestSerializer>();
            MemoryStream mem = new MemoryStream();
            await context.Request.Body.CopyToAsync(mem);
            var bytes = mem.ToArray();
            var proxyRequest = serializer.Deserialize<ProxyRequest>(bytes);

            var requestType = Type.GetType(proxyRequest.RequestType);
            if (requestType == null)
                throw new Exception();
            var request = serializer.Deserialize(proxyRequest.Request, requestType);
            if (request == null)
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
            throw new InvalidOperationException("Invalid request");
        }
    }
}