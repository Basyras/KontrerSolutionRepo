using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basyc.MessageBus.HttpProxy.Server.Asp;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebApplicationHttpProxyExtensions
    {
        public static WebApplication MapBusManagerProxy(this WebApplication app) => MapBusManagerProxy(app, "");
        public static WebApplication MapBusManagerProxy(this WebApplication app, string pattern) 
        {
            app.MapPost(pattern, Constants.ProxyHandler);
            return app;
        }
    }
}
