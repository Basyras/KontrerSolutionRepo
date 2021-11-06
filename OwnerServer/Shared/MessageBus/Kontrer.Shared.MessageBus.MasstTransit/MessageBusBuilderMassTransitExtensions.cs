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
        public static MessageBusBuilder AddMassTransitProvider(this MessageBusBuilder builder, bool hasHandlers = true)
        {
            var services = builder.services;
            services.AddSingleton<IMessageBusManager, MassTransitMessageBusManager>();
            services.AddTransient<IStartupFilter, MassTransitStartupFilter>();
            services.AddHealthChecks();
            services.AddMassTransit(x =>
            {
                if (hasHandlers)
                {
                    x.WrapRequestHandlersAsConsumers();
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