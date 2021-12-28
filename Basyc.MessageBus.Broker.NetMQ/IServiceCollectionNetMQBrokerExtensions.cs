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
        public static IServiceCollection AddNetMQMessageBroker(this IServiceCollection services, 
            string addressForSubscribers, int portForSubscribers,
            string addressForPublishers, int portForPublishers,
            int brokerServerPort = 5357, string brokerServerAddress = "localhost")
        {
            services.AddSingleton<IMessageBrokerServer, NetMQMessageBrokerServer>();
            services.Configure<NetMQMessageBrokerServerOptions>(x => 
            {
                x.AddressForSubscribers = addressForSubscribers;
                x.PortForSubscribers = portForSubscribers;
                x.AddressForPublishers = addressForPublishers;
                x.PortForPublishers = portForPublishers;
                x.BrokerServerAddress = brokerServerAddress;
                x.BrokerServerPort = brokerServerPort;
            });
            return services;
        }

    }
}
