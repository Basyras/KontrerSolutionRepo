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

namespace Basyc.MessageBus.Client
{
    public class MessageBusClientBuilder : DependencyBuilderBase
    {

        public MessageBusClientBuilder(IServiceCollection services) : base(services)
        {
            services.AddSingleton<ITypedMessageBusClient, TypedToSimpleMessageBusClient>();
        }

        public MessageBusClientBuilder RegisterTypedMessageHandlers<THandlerAssemblyMarker>()
        {
            return RegisterMessageHandlers(typeof(THandlerAssemblyMarker).Assembly);
        }

        public MessageBusClientBuilder RegisterMessageHandlers(params Assembly[] assembliesToScan)
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

            return this;
        }
    }
}