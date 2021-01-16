using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Dapr
{
    public static class IHostBuilderDaprExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureDapr(this IHostBuilder builder, Action<MicroserviceBuilder> configure)
        {

            builder.ConfigureWebHostDefaults(webBuilder =>
            {              

                webBuilder
                .ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
                {
                    services.AddDaprClient();
                    services.AddSingleton(new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                    });

                    services.AddControllers().AddDapr();              

                    services.Configure<DaprMessageBusManagerOptions>(options =>
                    {
                        options.PubSubName = MessageBusConstants.MessageBusName;
                    });

                    services.AddTransient<IStartupFilter, DaprStartupFilter>(); //Configure
                });

                var serviceBuilder = new MicroserviceBuilder(webBuilder).AddDaprProvider();
                configure(serviceBuilder);

            });
            return builder;
        }

    }
}
