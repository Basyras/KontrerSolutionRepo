using Basyc.MessageBus.RequestResponse;
using Basyc.Shared.Helpers;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using System;
using System.Linq;
using System.Reflection;

#warning should add answer with my approach

//https://stackoverflow.com/questions/52805079/how-to-register-a-generic-consumer-adapter-in-masstransit-if-i-have-a-list-of-me
namespace Basyc.MessageBus.MasstTransit
{
    public static class MassTransitConfiguratorExtensions
    {
        private static readonly Type[] requestHandlerInterfacesTypes;

        static MassTransitConfiguratorExtensions()
        {
            requestHandlerInterfacesTypes = new Type[] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };
        }

        public static void WrapRequestHandlersAsConsumers(this IServiceCollectionBusConfigurator busConfigurator)
        {
            var commandHandlers = busConfigurator.Collection
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, requestHandlerInterfacesTypes[0]))
                .Select(service => new
                {
                    RequestType = GenericsHelper.GetTypeArgumentsFromParent(service.ImplementationType, requestHandlerInterfacesTypes[0])[0]
                }).ToArray(); //ToArray because configurator.AddConsumer() is modifying services collection

            foreach (var commandHandler in commandHandlers)
            {
                Type proxyConsumerType = typeof(MassTransitBasycConsumerProxy<>).MakeGenericType(commandHandler.RequestType);
                busConfigurator.AddConsumer(proxyConsumerType);
            }

            var queryHandlers = busConfigurator.Collection
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, requestHandlerInterfacesTypes[1]))
                .Select(service => new
                {
                    RequestType = GenericsHelper.GetTypeArgumentsFromParent(service.ImplementationType, requestHandlerInterfacesTypes[1])[0],
                    ResponseType = GenericsHelper.GetTypeArgumentsFromParent(service.ImplementationType, requestHandlerInterfacesTypes[1])[1]
                }).ToArray(); //ToArray because configurator.AddConsumer() is modifying services collection

            foreach (var queryHandler in queryHandlers)
            {
                Type proxyConsumerType = typeof(MassTransitBasycConsumerProxy<,>).MakeGenericType(queryHandler.RequestType, queryHandler.ResponseType);
                busConfigurator.AddConsumer(proxyConsumerType);
            }
        }
    }
}