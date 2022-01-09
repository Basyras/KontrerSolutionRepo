using Basyc.MessageBus;
using Basyc.MessageBus.Client.Building;
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
        public static BusClientSelectMessageTypeStage AddBasycMessageBusClient(this IServiceCollection services)
        {
            return new BusClientSelectMessageTypeStage(services);
        }
    }
}