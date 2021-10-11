using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionMessageBusServerExtensions
    {
        public static IServiceCollection AddMessageBusProxyServer(this IServiceCollection services)
        {
            services.AddSingleton<IRequestSerializer, JsonRequestSerializer>();
            return services;
        }
    }
}