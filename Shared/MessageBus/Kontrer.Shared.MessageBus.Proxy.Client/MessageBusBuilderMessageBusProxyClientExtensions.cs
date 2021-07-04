using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.Proxy.Client;
using Kontrer.Shared.MessageBus.Proxy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessageBusBuilderMessageBusProxyClientExtensions
    {
        public static MessageBusProxyClientBuilder UseProxy(this MessageBusBuilder builder)
        {
            builder.services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
            return new MessageBusProxyClientBuilder(builder.services);
        }
    }
}