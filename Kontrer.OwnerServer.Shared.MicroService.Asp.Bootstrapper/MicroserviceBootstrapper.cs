using Kontrer.OwnerServer.Shared.Asp;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class MicroserviceBootstrapper
    {
        public static MicroserviceBuilder<IHostBuilder> CreateBuilder<TStartup>(string[] args)
            where TStartup : class, IStartupClass
        {
            var microserviceBuilder = Host.CreateDefaultBuilder()
                .CreateMicroserviceBuilder<TStartup>();

            return microserviceBuilder;
        }
    }
}