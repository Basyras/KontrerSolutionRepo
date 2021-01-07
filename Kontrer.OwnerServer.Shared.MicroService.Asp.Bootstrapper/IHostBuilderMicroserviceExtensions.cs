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

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class IHostBuilderMicroserviceExtensions
    {
        public static IHostBuilder ConfigureMicroservice<TStartup>(this IHostBuilder builder) where TStartup : class, IStartupClass
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(ConfigureMicroservicesDefaults);
                webBuilder.UseSetting(WebHostDefaults.ApplicationKey, entryAssembly.GetName().Name); //workaround because: https://github.com/dotnet/aspnetcore/issues/7315                      
                webBuilder.UseStartup<TStartup>();
            });

            builder.ConfigureDapr((MicroserviceBuilder serviceBuilder) =>
            {                             
                var actorRegistrator = new ActorRegistrator(serviceBuilder.MicroserviceProvider);
                actorRegistrator.RegisterActors<TStartup>();                
            });

            return builder;
        }

        private static void ConfigureMicroservicesDefaults(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = context.HostingEnvironment.ApplicationName, Version = "v1" });
            });

            services.AddSingleton<IMessageBusManager, DefaultMessageBusManager>();
            services.AddTransient<IStartupFilter, DefaultStartupFilter>(); //Configure
        }

    }
}
