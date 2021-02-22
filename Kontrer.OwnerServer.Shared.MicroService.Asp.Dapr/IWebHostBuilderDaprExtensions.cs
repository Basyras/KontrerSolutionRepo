﻿using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
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
    public static class IWebHostBuilderDaprExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webBuilder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureDapr(this IWebHostBuilder webBuilder, Action<MicroserviceBuilder> configure)
        {

            webBuilder.ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
            {
                services.AddTransient<IStartupFilter, DaprStartupFilter>();
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

            });

            var serviceBuilder = new MicroserviceBuilder(webBuilder).AddDaprProvider();
            configure(serviceBuilder);


            return webBuilder;
        }

    }
}