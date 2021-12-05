using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Monitoring.Health;
using Basyc.Asp;
using Basyc.MicroService.Abstraction.Initialization;

namespace Basyc.MicroService.Asp.Bootstrapper
{
    public static class IHostBuilderBootstrapperExtensions
    {
        private static readonly Assembly entryAssembly = Assembly.GetEntryAssembly();

        public static MicroserviceBuilder<IHostBuilder> CreateMicroserviceBuilder<TStartup>(this IHostBuilder hostBuilder) where TStartup : class, IStartupClass
        {
            ServiceCollection builderServices = new ServiceCollection();

            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                //Builder services needs to be manualy moved into web builder services
                webBuilder.ConfigureServices((s, aspServices) =>
                {
                    foreach (var service in builderServices)
                    {
                        aspServices.Add(service);
                    }
                });
                webBuilder.ConfigureAsp<TStartup>(entryAssembly.GetName().Name);
            });

            return new MicroserviceBuilder<IHostBuilder>(builderServices, hostBuilder);
        }
    }
}