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

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class IHostBuilderBootstrapperExtensions
    {
        private static readonly Assembly entryAssembly  = Assembly.GetEntryAssembly();

        public static IHostBuilder ConfigureServicesForMicroservice<TStartup>(this IHostBuilder builder) where TStartup : class, IStartupClass
        {
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(ConfigureMicroservicesDefaults);

                //workaround because: https://github.com/dotnet/aspnetcore/issues/7315                      
                webBuilder.UseSetting(WebHostDefaults.ApplicationKey, entryAssembly.GetName().Name); 
                webBuilder.UseStartup<TStartup>();

                webBuilder.ConfigureServicesForDapr((MicroserviceBuilder serviceBuilder) =>
                {
                    var actorRegistrator = new ActorRegistrator(serviceBuilder.MicroserviceProvider);
                    actorRegistrator.RegisterActors<TStartup>();
                });


            });          

            return builder;
        }

        private static void ConfigureMicroservicesDefaults(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, BootstrapperStartupFilter>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = context.HostingEnvironment.ApplicationName, Version = "v1" });
            });

            services.AddSingleton<IMessageBusManager, DefaultMessageBusManager>();
            services.AddMassTransit(x =>
            {
                
                x.AddConsumers(entryAssembly);
                
                x.UsingRabbitMq((transitContext, rabbitConfig) =>
                {

#warning finish automatic consumer registration
                    //var settings = new EndpointSettings<ConsumerEndpointDefinition<IConsumer>>();
                    //new ConsumerEndpointDefinition<IConsumer>(settings)
                    //var definition = new NamedEndpointDefinition(context.HostingEnvironment.ApplicationName);
                    rabbitConfig.ConfigureEndpoints(transitContext);
                    //rabbitConfig.ReceiveEndpoint(context.HostingEnvironment.ApplicationName, c =>
                    //{                    
                    //});
                    rabbitConfig.UseHealthCheck(transitContext);

                });

                
                services.AddMassTransitHostedService();

            });
        }

    }
}
