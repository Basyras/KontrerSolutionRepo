using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.HttpProxy.Client;
using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessageBusBuilderHttpProxyClientExtensions
    {
        public static MessageBusHttpProxyClientBuilder AddProxyProvider(this MessageBusBuilder builder)
        {
            builder.services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
            builder.services.AddSingleton(new HttpClient());
            return new MessageBusHttpProxyClientBuilder(builder.services);
        }
    }
}