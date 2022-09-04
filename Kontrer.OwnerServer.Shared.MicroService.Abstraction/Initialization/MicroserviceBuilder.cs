﻿using Basyc.DependencyInjection;
using Basyc.MessageBus.Client.Building;
using Basyc.MicroService.Abstraction.Initialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class MicroserviceBuilder<TParentBuilder> : DependencyBuilderBase<TParentBuilder>
    {
        public MicroserviceBuilder(IServiceCollection services, TParentBuilder parentBuilder) : base(services, parentBuilder)
        {
            
        }

        public IMicroserviceProvider MicroserviceProvider { get; private set; }

        public MicroserviceBuilder<TParentBuilder> AddProvider(IMicroserviceProvider provider)
        {
            MicroserviceProvider = provider;
            return this;
        }

        public BusClientSelectMessageTypeStage AddMessageBus()
        {
            return services.AddBasycBusClient();
        }
    }
}