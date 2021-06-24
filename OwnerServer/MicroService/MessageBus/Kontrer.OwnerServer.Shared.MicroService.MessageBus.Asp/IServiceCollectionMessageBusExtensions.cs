using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kontrer.OwnerServer.Shared.MicroService.MessageBus.Asp
{
    public static class IServiceCollectionMessageBusExtensions
    {
        //public static IServiceCollection RegisterCommnandQueriesEndpoints(this IServiceCollection services)
        //{
        //    services.AddTransient<IStartupFilter, AspMessageBusStartupFilter>();
        //    return services;
        //}

        public static IWebHostBuilder RegisterCommnandQueriesEndpoints<TCommand>(this IWebHostBuilder webBuilder)
        {
            webBuilder.ConfigureServices((WebHostBuilderContext context, IServiceCollection services) =>
            {
                services.AddTransient<IStartupFilter, AspMessageBusStartupFilter<TCommand>>();
            });

            return webBuilder;
        }
    }
}