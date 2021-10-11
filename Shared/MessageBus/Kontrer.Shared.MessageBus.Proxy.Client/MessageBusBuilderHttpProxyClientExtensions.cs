using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.HttpProxy.Client;
using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessageBusBuilderHttpProxyClientExtensions
    {
        public static MessageBusHttpProxyClientBuilder UseProxy(this MessageBusBuilder builder)
        {
            builder.services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
            return new MessageBusHttpProxyClientBuilder(builder.services);
        }
    }
}