﻿using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.HttpProxy.Client
{
    public class MessageBusHttpProxyClientBuilder
    {
        private readonly IServiceCollection services;

        public MessageBusHttpProxyClientBuilder(IServiceCollection services)
        {
            services.AddSingleton<IMessageBusManager, HttpProxyClientMessageBusManager>();
            this.services = services;
        }

        public MessageBusHttpProxyClientBuilder UseSerializer<TSerializer>() where TSerializer : class, IRequestSerializer
        {
            services.AddSingleton<TSerializer>();
            return this;
        }

        public MessageBusHttpProxyClientBuilder SetProxyServerUri(Uri hostUri)
        {
            services.Configure<MessageBusHttpProxyClientOptions>(x => x.ProxyHostUri = hostUri);
            return this;
        }
    }
}