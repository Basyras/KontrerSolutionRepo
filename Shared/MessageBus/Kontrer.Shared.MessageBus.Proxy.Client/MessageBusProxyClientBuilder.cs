using Kontrer.Shared.MessageBus.Proxy.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Proxy.Client
{
    public class MessageBusProxyClientBuilder
    {
        private readonly IServiceCollection services;

        public MessageBusProxyClientBuilder(IServiceCollection services)
        {
            services.AddSingleton<IMessageBusManager, ProxyClientMessageBusManager>();
            this.services = services;
        }

        public MessageBusProxyClientBuilder UseSerializer<TSerializer>() where TSerializer : class, IRequestSerializer
        {
            services.AddSingleton<TSerializer>();
            return this;
        }

        public MessageBusProxyClientBuilder SetProxyServerUri(Uri hostUri)
        {
            services.Configure<MessageBusProxyClientOptions>(x => x.ProxyHostUri = hostUri);
            return this;
        }
    }
}