using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.MasstTransit;
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

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessageBusBuilderMassTransitExtensions
    {
        /// <summary>
        /// Takes registered Basyc IRequestHandlers and wrap them with MassTransit IConsumers, Hosted by RabbitMQ
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="handlersAssemblies"></param>
        /// <returns></returns>
        public static MessageBusBuilder AddMassTransitProvider(this MessageBusBuilder builder)
        {
            return AddMassTransitProvider(builder, null);
        }

        public static MessageBusBuilder AddMassTransitProvider(this MessageBusBuilder builder, params Assembly[] handlersAssemblies)
        {
            var services = builder.services;
            services.AddSingleton<IMessageBusManager, MassTransitMessageBusManager>();
            services.AddTransient<IStartupFilter, MassTransitStartupFilter>();
            services.AddHealthChecks();
            services.AddMassTransit(x =>
            {
                if (handlersAssemblies != null && handlersAssemblies.Any())
                {
                    x.WrapRequestHandlersAsConsumers(handlersAssemblies);
                }
                x.UsingRabbitMq((transitContext, rabbitConfig) =>
                {
                    rabbitConfig.ConfigureEndpoints(transitContext);
                });
            });
            services.AddMassTransitHostedService();
            return builder;
        }
    }
}