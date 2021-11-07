using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.HttpProxy.Server.Asp;
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
        private static IMessageBusManager messageBus;
        private static IRequestSerializer serializer;
        private static ProxyHttpReqeustHandler httpHandler;

        public static void MapMessageBusProxyServer(this IEndpointRouteBuilder endpoints)
        {
            httpHandler = endpoints.ServiceProvider.GetRequiredService<ProxyHttpReqeustHandler>();

            endpoints.MapPost("", async (HttpContext context) =>
            {
                try
                {
                    await httpHandler.Handle(context);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(ex.Message);
                }
            });
        }
    }
}