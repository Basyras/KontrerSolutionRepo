using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.HttpProxy.Server.Asp
{
    public static class Constants
    {
        public static readonly RequestDelegate ProxyHandler = async (HttpContext context) =>
        {
            var httpHandler = context.RequestServices.GetRequiredService<ProxyHttpReqeustHandler>();
            try
            {
                await httpHandler.Handle(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(ex.Message);
            }
        };
    }
}
