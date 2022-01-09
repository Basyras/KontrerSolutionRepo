using Basyc.MessageBus.HttpProxy.Server.Asp;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.Serialization.Abstraction;
using Basyc.Serialization.ProtobufNet;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionMessageBusServerExtensions
    {
        public static IServiceCollection AddMessageBusProxyServer(this IServiceCollection services)
        {
            //services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
            services.AddSingleton<ITypedByteSerializer, ProtobufByteSerializer>();
            services.AddSingleton<ISimpleByteSerailizer, SimpleFromTypedSerializer>();

            services.AddSingleton<ProxyHttpReqeustHandler>();
            return services;
        }
    }
}