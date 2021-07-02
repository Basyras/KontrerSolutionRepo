using Kontrer.OwnerServer.Shared.MessageBus;
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

namespace Kontrer.OwnerServer.Shared.MessageBus.MasstTransit
{
    public static class IWebHostBuilderMassTransitExtensions
    {
        public static IWebHostBuilder ConfigureMassTransitServices(this IWebHostBuilder webBuilder, Assembly handlersAssembly)
        {
            webBuilder.ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
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
            });

            return webBuilder;
        }
    }
}