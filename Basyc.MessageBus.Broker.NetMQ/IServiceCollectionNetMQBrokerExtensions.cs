using Basyc.MessageBus.NetMQ.Shared;
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
            int brokerServerPort = 5357, string brokerServerAddress = "localhost")
        {
            services.AddSingleton<IMessageBrokerServer, NetMQMessageBrokerServer>();
            services.AddSingleton<IWorkerRegistry, WorkerRegistry>();
            services.AddSingleton<IMessageToByteSerializer, TypedMessageToByteSerializer>();
            services.Configure<NetMQMessageBrokerServerOptions>(x => 
            {
                x.BrokerServerAddress = brokerServerAddress;
                x.BrokerServerPort = brokerServerPort;
            });
            return services;
        }

    }
}
