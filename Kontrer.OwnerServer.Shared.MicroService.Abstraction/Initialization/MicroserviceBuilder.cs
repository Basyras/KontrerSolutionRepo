using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization
{
    public class MicroserviceBuilder
    {
        public IWebHostBuilder WebBuilder { get; }

        public MicroserviceBuilder(IWebHostBuilder builder)
        {
            this.WebBuilder = builder;
        }

        public IMicroserviceProvider MicroserviceProvider { get; private set; }
        public MicroserviceBuilder AddProvider(IMicroserviceProvider provider)
        {
            this.MicroserviceProvider = provider;
            return this;
        }





    }
}
