using Basyc.DependencyInjection;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.Building
{
    public class BusClientSelectMessageTypeStage : BuilderStageBase
    {

        public BusClientSelectMessageTypeStage(IServiceCollection services) : base(services)
        {

        }

        public BusClientSetupTypedHandlersStage WithTypedMessages()
        {
            services.AddSingleton<ITypedMessageBusClient, TypedToSimpleMessageBusClient>();
            return new BusClientSetupTypedHandlersStage(services);
        }

        public BusClientSetupTypedHandlersStage WithSimpleMessages()
        {
            return new BusClientSetupTypedHandlersStage(services);
        }
    }
}