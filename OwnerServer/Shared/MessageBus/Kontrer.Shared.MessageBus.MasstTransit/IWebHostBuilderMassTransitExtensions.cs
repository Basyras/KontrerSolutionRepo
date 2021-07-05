using Kontrer.Shared.MessageBus;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.MasstTransit
{
    public static class IWebHostBuilderMassTransitExtensions
    {
        public static IWebHostBuilder ConfigureMassTransitServices(this IWebHostBuilder webBuilder, Assembly handlersAssembly)
        {
            webBuilder.ConfigureServices((context, services) => ConfigureMassTransit(services, handlersAssembly));

            return webBuilder;
        }

        public static MessageBusBuilder UseMassTransit(this MessageBusBuilder builder, Assembly handlersAssembly)
        {
            ConfigureMassTransit(builder.services, handlersAssembly);
            return builder;
        }

        private static void ConfigureMassTransit(IServiceCollection services, Assembly handlersAssembly)
        {
            services.AddSingleton<IMessageBusManager, MassTransitMessageBusManager>();
            services.AddTransient<IStartupFilter, MasstransitStartupFilter>();
            services.AddHealthChecks();
            services.AddMassTransit(x =>
            {
                //x.AddConsumers(consumerAssemblies);
                x.AddConsumersFromMessageBus(handlersAssembly);
                x.UsingRabbitMq((transitContext, rabbitConfig) =>
                {
                    rabbitConfig.ConfigureEndpoints(transitContext);
                    //rabbitConfig.ReceiveEndpoint(context.HostingEnvironment.ApplicationName, c =>
                    //{
                    //});
                    //rabbitConfig.ReceiveEndpoint(context.HostingEnvironment.ApplicationName, ep =>
                    //{
                    //});
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}