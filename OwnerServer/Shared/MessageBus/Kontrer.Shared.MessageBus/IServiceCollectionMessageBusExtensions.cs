using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus
{
    public static class IServiceCollectionMessageBusExtensions
    {
        public static MessageBusBuilder AddMessageBus(this IServiceCollection services)
        {
            return new MessageBusBuilder(services);
        }
    }
}