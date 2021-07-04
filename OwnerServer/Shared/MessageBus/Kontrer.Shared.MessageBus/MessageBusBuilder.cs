using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus
{
    public class MessageBusBuilder
    {
        public readonly IServiceCollection services;

        public MessageBusBuilder(IServiceCollection services)
        {
            this.services = services;
        }
    }
}