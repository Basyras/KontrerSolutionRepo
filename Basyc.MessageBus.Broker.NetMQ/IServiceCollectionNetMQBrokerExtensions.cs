using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Broker.NetMQ
{
    public static class IServiceCollectionNetMQBrokerExtensions
    {
        public static IServiceCollection AddNetMQMessageBroker(this IServiceCollection services, int portForSubscribers, int portForPublishers)
        {
            services.AddSingleton<IMessageBroker, NetMQMessageBroker>();
            services.Configure<NetMQMessageBrokerOptions>(x => 
            {
                x.PortForSubscribers = portForSubscribers;
                x.PortForPublishers = portForPublishers;
            });
            return services;
        }

    }
}
