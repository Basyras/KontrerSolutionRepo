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
using MassTransit;
using MassTransit.Definition;
using MassTransit.Monitoring.Health;
using Kontrer.OwnerServer.Shared.Asp;
using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.Asp;
using Kontrer.Shared.MessageBus.MasstTransit;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class IHostBuilderBootstrapperExtensions
    {
        private static readonly Assembly entryAssembly = Assembly.GetEntryAssembly();

        public static MicroserviceBuilder<IHostBuilder> CreateMicroserviceBuilder<TStartup>(this IHostBuilder hostBuilder) where TStartup : class, IStartupClass
        {
            ServiceCollection services = new ServiceCollection();
            //hostBuilder.ConfigureServices((s, a) =>
            //{
            //    foreach (var service in services)
            //    {
            //        a.Add(service);
            //    }
            //});

            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                //webBuilder.ConfigureServices((s, a) =>
                //{
                //    foreach (var service in services)
                //    {
                //        a.Add(service);
                //    }
                //});
                webBuilder.ConfigureAsp<TStartup>(entryAssembly.GetName().Name);
                //webBuilder.UseStartupWorkaround<TStartup>(entryAssembly.GetName().Name);

                //webBuilder.ConfigureMicroservice();
                //webBuilder.ConfigureMicroservice()
                //.AddMessageBus(handlersAssembly);
                //webBuilder.ConfigureServices(services =>
                //{
                //    services.AddMessageBus()
                //    .RegisterRequestHandlers(handlersAssembly);
                //});
                //webBuilder.ConfigureMessageBusServices<TStartup>(handlersAssembly);
            });

            return new MicroserviceBuilder<IHostBuilder>(services, hostBuilder);
        }

        private static IWebHostBuilder ConfigureMessageBusServices<TStartup>(this IWebHostBuilder webBuilder, Assembly handlersAssembly) where TStartup : class, IStartupClass
        {
            //?
            //webBuilder.RegisterHandlersToDI(handlersAssembly);

            //webBuilder.ConfigureMassTransitServices(handlersAssembly);
            //webBuilder.ConfigureDaprServices((MicroserviceBuilder<IHostBuilder> serviceBuilder) =>
            //{
            //    var actorRegistrator = new ActorRegistrator(serviceBuilder.MicroserviceProvider);
            //    actorRegistrator.RegisterActors<TStartup>();
            //});

            //?
            //webBuilder.RegisterCommnandQueriesEndpoints<TCommand>();
            return webBuilder;
        }
    }
}