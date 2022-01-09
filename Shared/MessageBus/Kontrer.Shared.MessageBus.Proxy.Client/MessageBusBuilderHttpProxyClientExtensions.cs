using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.HttpProxy.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Microsoft.Extensions.DependencyInjection;
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
        public static MessageBusHttpProxyClientBuilder AddProxyClient(this BusClientSelectMessageTypeStage builder)
        {
            builder.services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
            builder.services.AddSingleton(new HttpClient());
            return new MessageBusHttpProxyClientBuilder(builder.services);
        }
    }
}