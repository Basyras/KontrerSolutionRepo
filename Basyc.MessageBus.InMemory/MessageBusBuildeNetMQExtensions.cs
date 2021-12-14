using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.NetMQ;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.Shared.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class MessageBusBuildeNetMQExtensions
{
    //static MethodInfo handleMethodInfo = typeof(IMessageHandler<>).MakeGenericType().GetMethod(nameof(IMessageHandler<IMessage>.Handle))!;
    //static MethodInfo handleWithResponseMethodInfo = typeof(IMessageHandler<,>).GetMethod(nameof(IMessageHandler<IMessage<object>,object>.Handle))!;

    public static MessageBusClientBuilder AddNetMQProvider(this MessageBusClientBuilder builder, int portForPublishers, int portForSubscribers, int portForPush, int portForPull)
    {
        var services = builder.services;
        services.AddSingleton<IMessageBusClient, NetMQMessageBusClient>();
        services.Configure<NetMQMessageBusClientOptions>(x =>
        {
            x.PortForSubscribers = portForSubscribers;
            x.PortForPublishers = portForPublishers;
            x.PortForPush = portForPush;
            x.PortForPull = portForPull;

            var messageHandlerTypes = builder.services
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<>)));

            foreach (var messageHandlerService in messageHandlerTypes)
            {
                Type handlerType = messageHandlerService.ImplementationType!;
                Type messageType = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<>))[0];
                MethodInfo handleMethodInfo = typeof(IMessageHandler<>).MakeGenericType(messageType).GetMethod(nameof(IMessageHandler<IMessage>.Handle))!;
                x.Handlers.Add(new MessageHandlerInfo(handlerType,messageType, handleMethodInfo));
            }

            var messagesWithResponse = builder.services
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<,>)));

            foreach (var messageHandlerServiceWithResponse in messagesWithResponse)
            {
                Type handlerType = messageHandlerServiceWithResponse.ImplementationType!;
                Type[] typeArguments = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<,>));
                Type messageType = typeArguments[0];
                Type responseType = typeArguments[1];
                MethodInfo handleWithResponseMethodInfo = typeof(IMessageHandler<,>)
                .MakeGenericType(messageType, responseType)
                .GetMethod(nameof(IMessageHandler<IMessage<object>, object>.Handle))!;
                //.MakeGenericMethod(responseType)!;
                //MethodInfo generichandleMethodInfo = handleWithResponseMethodInfo.MakeGenericMethod(responseType)!;
                x.Handlers.Add(new MessageHandlerInfo(handlerType, messageType, handleWithResponseMethodInfo));
            }
        });
        return builder;
    }


}
