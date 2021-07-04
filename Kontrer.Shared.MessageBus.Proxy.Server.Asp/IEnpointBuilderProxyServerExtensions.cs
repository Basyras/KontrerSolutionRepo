using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.Proxy.Shared;
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
    public static class IEnpointBuilderProxyServerExtensions
    {
        public static void MapMessageBusProxyServer(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("", HandleRequest);
        }

        private static async Task HandleRequest(HttpContext context)
        {
            IMessageBusManager messageBus = context.RequestServices.GetRequiredService<IMessageBusManager>();
            var serializer = context.RequestServices.GetRequiredService<IRequestSerializer>();
            //var proxyRequest = await context.Request.ReadFromJsonAsync<ProxyRequest>();
            MemoryStream mem = new MemoryStream();
            await context.Request.Body.CopyToAsync(mem);
            var bytes = mem.ToArray();
            var proxyRequest = serializer.Deserialize<ProxyRequest>(bytes);
            var requestType = Type.GetType(proxyRequest.RequestType);
            await messageBus.SendAsync(requestType);
            //await context.Response.WriteAsync(serializer.Serialize(busResponse));
        }
    }
}