﻿using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
    public class BusManagerBuilder
    {
        public readonly IServiceCollection services;

        public BusManagerBuilder(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IMessageManager, MessageManager>();
        }

        public BusManagerBuilder AddProvider<TDomainProvider>() where TDomainProvider : class, IDomainInfoProvider
        {
            services.AddSingleton<IDomainInfoProvider, TDomainProvider>();
            return this;
        }

        public BusManagerBuilder AddReqeustClient<TRequestClient>() where TRequestClient : class, IRequestClient
        {
            services.AddSingleton<IRequestClient, TRequestClient>();
            return this;
        }
    }
}