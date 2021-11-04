using Basyc.DependencyInjection;
using Kontrer.Shared.MessageBus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization
{
    public class MicroserviceBuilder<TParentBuilder> : DependencyBuilderBase<TParentBuilder>
    {
        public readonly IServiceCollection services;

        //public IHostBuilder webBuilder;

        //public MicroserviceBuilder(IHostBuilder hostBuilder) : base(null, hostBuilder)
        //{
        //    this.webBuilder = hostBuilder;
        //}
        public MicroserviceBuilder(IServiceCollection services, TParentBuilder parentBuilder) : base(services, parentBuilder)
        {
            this.services = services;
        }

        public IMicroserviceProvider MicroserviceProvider { get; private set; }

        public MicroserviceBuilder<TParentBuilder> AddProvider(IMicroserviceProvider provider)
        {
            this.MicroserviceProvider = provider;
            return this;
        }

        public MessageBusBuilder AddMessageBus(Assembly assembliesToScan)
        {
            //webBuilder.ConfigureServices(services =>
            //{
            //    services.AddMessageBus()
            //        .RegisterRequestHandlers(assembliesToScan);
            //});

            return services.AddMessageBus()
                 .RegisterRequestHandlers(assembliesToScan);
        }
    }
}