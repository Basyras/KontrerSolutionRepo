using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MessageBus;
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

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MassTransit
{
    public static class IWebHostBuilderMassTransitExtensions
    {
        public static IWebHostBuilder ConfigureMassTransitServices(this IWebHostBuilder webBuilder, params Assembly[] consumerAssemblies)
        {
            webBuilder.ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
            {
                services.AddTransient<IStartupFilter, MasstransitStartupFilter>();
                services.AddSingleton<IMessageBusManager, DefaultMessageBusManager>();

                services.AddHealthChecks();
                //services.Configure<HealthCheckPublisherOptions>(options =>
                //{
                //    options.Delay = TimeSpan.FromSeconds(2);
                //    options.Predicate = (check) => check.Tags.Contains("ready");
                //});

                services.AddMassTransit(x =>
                {
                    x.AddConsumers(consumerAssemblies);
                    x.UsingRabbitMq((transitContext, rabbitConfig) =>
                    {
#warning finish automatic consumer registration
                        //var settings = new EndpointSettings<ConsumerEndpointDefinition<IConsumer>>();
                        //new ConsumerEndpointDefinition<IConsumer>(settings)
                        //var definition = new NamedEndpointDefinition(context.HostingEnvironment.ApplicationName);
                        rabbitConfig.ConfigureEndpoints(transitContext);
                        //rabbitConfig.UseHealthCheck(transitContext);
                        //rabbitConfig.ReceiveEndpoint(context.HostingEnvironment.ApplicationName, c =>
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