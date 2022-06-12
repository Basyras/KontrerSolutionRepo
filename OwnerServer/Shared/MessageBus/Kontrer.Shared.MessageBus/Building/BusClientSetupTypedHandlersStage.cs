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
    public class BusClientSetupTypedHandlersStage : BuilderStageBase
    {
        public BusClientSetupTypedHandlersStage(IServiceCollection services) : base(services)
        {

        }

        public BusClientSelectClientProviderStage NoHandlers()
        {
            return new BusClientSelectClientProviderStage(services);
        }

        public BusClientSelectClientProviderStage RegisterBasycTypedHandlers<THandlerAssemblyMarker>()
        {
            return RegisterBasycHandlers(typeof(THandlerAssemblyMarker).Assembly);
        }

        public BusClientSelectClientProviderStage RegisterBasycHandlers(params Assembly[] assembliesToScan)
        {
            services.Scan(scan =>
            scan.FromAssemblies(assembliesToScan)
            .AddClasses(classes => classes.AssignableTo(typeof(IMessageHandler<>)))
            .As(handler => new Type[1]
            {
                typeof(IMessageHandler<>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handler, typeof(IMessageHandler<>)))
            })
            .WithScopedLifetime()

            .AddClasses(classes => classes.AssignableTo(typeof(IMessageHandler<,>)))
            .As(handler => new Type[1]
            {
                typeof(IMessageHandler<,>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handler, typeof(IMessageHandler<,>)))
            })
            .WithScopedLifetime());

            return new BusClientSelectClientProviderStage(services);
        }


    }
}
