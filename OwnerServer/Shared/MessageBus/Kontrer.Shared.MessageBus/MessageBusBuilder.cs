using Basyc.DependencyInjection;
using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus
{
    public class MessageBusBuilder : DependencyBuilderBase
    {
        public MessageBusBuilder(IServiceCollection services) : base(services)
        {
        }

        public MessageBusBuilder RegisterBasycRequestHandlers<THandlerAssemblyMarker>()
        {
            return RegisterBasycRequestHandlers(typeof(THandlerAssemblyMarker).Assembly);
        }

        public MessageBusBuilder RegisterBasycRequestHandlers(params Assembly[] assembliesToScan)
        {
            this.services.Scan(scan =>
            scan.FromAssemblies(assembliesToScan)
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
            .As(handler => new Type[1]
            {
                typeof(IRequestHandler<>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handler, typeof(IRequestHandler<>)))
            })
            .WithScopedLifetime()

            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .As(handler => new Type[1]
            {
                typeof(IRequestHandler<,>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handler, typeof(IRequestHandler<,>)))
            })
            .WithScopedLifetime());

            return this;
        }
    }
}