using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus.RequestResponse;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using System;
using System.Linq;
using System.Reflection;

#warning should add answer with my approach

//https://stackoverflow.com/questions/52805079/how-to-register-a-generic-consumer-adapter-in-masstransit-if-i-have-a-list-of-me
namespace Kontrer.Shared.MessageBus.MasstTransit
{
    public static class MassTransitConfiguratorExtensions
    {
        private static readonly Type[] requestHandlerInterfacesTypes;

        static MassTransitConfiguratorExtensions()
        {
            requestHandlerInterfacesTypes = new Type[] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };
        }

        //public static void WrapRequestHandlersAsConsumers(this IServiceCollectionBusConfigurator configurator, params Assembly[] handlersAssemblies)
        //{
        //    var assemblyTypes = handlersAssemblies.SelectMany(x => x.GetTypes());

        //    var commandHandlers = assemblyTypes.Where(sendHandlerType => GenericsHelper.IsAssignableToGenericType(sendHandlerType, requestHandlerInterfacesTypes[0]))
        //        .Select(handlerType => new
        //        {
        //            RequestType = GenericsHelper.GetGenericArgumentsFromParent(handlerType, requestHandlerInterfacesTypes[0])[0]
        //        });

        //    foreach (var commandHandler in commandHandlers)
        //    {
        //        Type proxyConsumer;
        //        proxyConsumer = typeof(MassTransitBasycConsumerProxy<>).MakeGenericType(commandHandler.RequestType);
        //        configurator.AddConsumer(proxyConsumer);
        //    }

        //    var queryHandlers = assemblyTypes.Where(requestHandlerType => GenericsHelper.IsAssignableToGenericType(requestHandlerType, requestHandlerInterfacesTypes[1]))
        //            .Select(handlerType => new
        //            {
        //                RequestType = GenericsHelper.GetGenericArgumentsFromParent(handlerType, requestHandlerInterfacesTypes[1])[0],
        //                ResponseType = GenericsHelper.GetGenericArgumentsFromParent(handlerType, requestHandlerInterfacesTypes[1])[1]
        //            });

        //    foreach (var queryHandler in queryHandlers)
        //    {
        //        Type proxyConsumer;
        //        proxyConsumer = typeof(MassTransitBasycConsumerProxy<,>).MakeGenericType(queryHandler.RequestType, queryHandler.ResponseType);
        //        configurator.AddConsumer(proxyConsumer);
        //    }
        //}

        public static void WrapRequestHandlersAsConsumers(this IServiceCollectionBusConfigurator configurator)
        {
            var commandHandlers = configurator.Collection
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, requestHandlerInterfacesTypes[0]))
                .Select(service => new
                {
                    RequestType = GenericsHelper.GetTypeArgumentsFromParent(service.ImplementationType, requestHandlerInterfacesTypes[0])[0]
                }).ToArray(); //ToArray because configurator.AddConsumer() is modifying services collection

            foreach (var commandHandler in commandHandlers)
            {
                Type proxyConsumer;
                proxyConsumer = typeof(MassTransitBasycConsumerProxy<>).MakeGenericType(commandHandler.RequestType);
                configurator.AddConsumer(proxyConsumer);
            }

            var queryHandlers = configurator.Collection
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, requestHandlerInterfacesTypes[1]))
                .Select(service => new
                {
                    RequestType = GenericsHelper.GetTypeArgumentsFromParent(service.ImplementationType, requestHandlerInterfacesTypes[1])[0],
                    ResponseType = GenericsHelper.GetTypeArgumentsFromParent(service.ImplementationType, requestHandlerInterfacesTypes[1])[1]
                }).ToArray(); //ToArray because configurator.AddConsumer() is modifying services collection

            foreach (var queryHandler in queryHandlers)
            {
                Type proxyConsumer;
                proxyConsumer = typeof(MassTransitBasycConsumerProxy<,>).MakeGenericType(queryHandler.RequestType, queryHandler.ResponseType);
                configurator.AddConsumer(proxyConsumer);
            }
        }
    }
}