﻿using Kontrer.Shared.Helpers;
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
        private static readonly Type[] requestIntefaceTypes;
        private static readonly Type[] requestHandlerInterfacesTypes;

        static MassTransitConfiguratorExtensions()
        {
            requestIntefaceTypes = new Type[] { typeof(IRequest), typeof(IRequest<>) };
            requestHandlerInterfacesTypes = new Type[] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };
        }

        public static void AddConsumersFromMessageBus(this IServiceCollectionBusConfigurator configurator, Assembly handlersAssembly)
        {
            var assemblyTypes = handlersAssembly.GetTypes();

            var sendHandlers = assemblyTypes.Where(sendHandlerType => GenericsHelper.IsAssignableToGenericType(sendHandlerType, requestHandlerInterfacesTypes[0]))
                .Select(handlerType => new
                {
                    HandlerType = handlerType,
                    RequestType = GenericsHelper.GetGenericArgumentsFromParent(handlerType, requestHandlerInterfacesTypes[0])[0]
                });

            foreach (var senderHandler in sendHandlers)
            {
                Type proxyConsumer;
                proxyConsumer = typeof(MassTransitGenericConsumerProxy<>).MakeGenericType(senderHandler.RequestType);
                configurator.AddConsumer(proxyConsumer);
            }

            var requestHandlers = assemblyTypes.Where(requestHandlerType => GenericsHelper.IsAssignableToGenericType(requestHandlerType, requestHandlerInterfacesTypes[1]))
                    .Select(handlerType => new
                    {
                        HandlerType = handlerType,
                        RequestType = GenericsHelper.GetGenericArgumentsFromParent(handlerType, requestHandlerInterfacesTypes[1])[0],
                        ResponseType = GenericsHelper.GetGenericArgumentsFromParent(handlerType, requestHandlerInterfacesTypes[1])[1]
                    });

            foreach (var requestHandler in requestHandlers)
            {
                Type proxyConsumer;
                proxyConsumer = typeof(MassTransitGenericConsumerProxy<,>).MakeGenericType(requestHandler.RequestType, requestHandler.ResponseType);
                configurator.AddConsumer(proxyConsumer);
            }
        }
    }
}