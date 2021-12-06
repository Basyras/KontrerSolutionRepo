using Basyc.MessageBus;
using Basyc.MessageBus.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionMessageBusExtensions
    {
        public static MessageBusClientBuilder AddMessageBusClient(this IServiceCollection services)
        {
            return new MessageBusClientBuilder(services);
        }
    }
}