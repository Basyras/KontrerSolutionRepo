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
    public class MessageBusBuilder
    {
        public readonly IServiceCollection services;

        public MessageBusBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public MessageBusBuilder RegisterRequestHandlers(Assembly assembliesToScan)
        {
            this.services.Scan(scan =>
            scan.FromAssemblies(assembliesToScan)
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
            .As(handler => new Type[1]
            {
                typeof(IRequestHandler<>).MakeGenericType(GenericsHelper.GetGenericArgumentsFromParent(handler, typeof(IRequestHandler<>)))
            })
            .WithScopedLifetime()

            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .As(handler => new Type[1]
            {
                typeof(IRequestHandler<,>).MakeGenericType(GenericsHelper.GetGenericArgumentsFromParent(handler, typeof(IRequestHandler<,>)))
            })
            .WithScopedLifetime());

            return this;
        }
    }
}