using Basyc.MessageBus.Client.RequestResponse;
using Basyc.Shared.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Basyc.MessageBus.Client.NetMQ;

public static class MessageBusBuildeNetMQExtensions
{
    static MethodInfo handleMethodInfo = typeof(IMessageHandler<>).GetMethod(nameof(IMessageHandler<IMessage>.Handle))!;
    static MethodInfo handleWithResponseMethodInfo = typeof(IMessageHandler<>).GetMethod(nameof(IMessageHandler<IMessage<object>,object>.Handle))!;

    public static MessageBusClientBuilder AddNetMQProvider(this MessageBusClientBuilder builder)
    {
        var services = builder.services;
        services.AddSingleton<IMessageBusClient, NetMQMessageBusClient>();
        services.Configure<NetMQMessageBusClientOptions>(x =>
        {
            var messageHandlerTypes = builder.services
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<>)));

            foreach (var messageHandlerService in messageHandlerTypes)
            {
                Type handlerType = messageHandlerService.ImplementationType!;
                Type messageType = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<,>))[0];
                MethodInfo handleMethod = handleMethodInfo.MakeGenericMethod(messageType)!;
                x.Handlers.Add(new MessageHandlerInfo(handlerType,messageType,handleMethod));
            }

            var messagesWithResponse = builder.services
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<,>)));

            foreach (var messageHandlerServiceWithResponse in messagesWithResponse)
            {
                Type handlerType = messageHandlerServiceWithResponse.ImplementationType!;
                Type[] typeArguments = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<,>));
                Type messageType = typeArguments[0];
                Type responseType = typeArguments[1];
                MethodInfo handleMethod = handleMethodInfo.MakeGenericMethod(messageType)!;
                x.Handlers.Add(new MessageHandlerInfo(handlerType, messageType, handleMethod));
            }
        });
        return builder;
    }


}
