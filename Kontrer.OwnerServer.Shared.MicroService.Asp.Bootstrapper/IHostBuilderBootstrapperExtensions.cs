using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Dapr;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Monitoring.Health;
using Kontrer.OwnerServer.Shared.Asp;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MassTransit;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class IHostBuilderBootstrapperExtensions
    {
        private static readonly Assembly entryAssembly = Assembly.GetEntryAssembly();

        public static IHostBuilder ConfigureMicroservice<TStartup>(this IHostBuilder hostBuilder, Assembly consumerAssembly) where TStartup : class, IStartupClass
        {
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAspServices();
                webBuilder.UseStartupWorkaround<TStartup>(entryAssembly.GetName().Name);
                webBuilder.ConfigureMicroServiceServices<TStartup>(consumerAssembly);
            });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureMicroServiceServices<TStartup>(this IWebHostBuilder webBuilder, Assembly consumersAssembly) where TStartup : class, IStartupClass
        {
            webBuilder.ConfigureMassTransitServices(consumersAssembly);
            webBuilder.ConfigureDaprServices((MicroserviceBuilder serviceBuilder) =>
            {
                var actorRegistrator = new ActorRegistrator(serviceBuilder.MicroserviceProvider);
                actorRegistrator.RegisterActors<TStartup>();
            });
            return webBuilder;
        }
    }
}