using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.NetMQ;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Shared.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class MessageBusClientBuilderNetMQExtensions
{
    public static MessageBusClientBuilder AddNetMQClient(this MessageBusClientBuilder builder,
       int portForPublishers, int portForSubscribers,int brokerServerPort) => 
        AddNetMQClient(builder, portForPublishers, portForSubscribers, brokerServerPort);

    public static MessageBusClientBuilder AddNetMQClient(this MessageBusClientBuilder builder, 
        int portForPublishers, int portForSubscribers,        
        string? clientId, int brokerServerPort = 5357, string brokerServerAddress = "localhost")
    {
        var services = builder.services;
        services.AddSingleton<ISimpleMessageBusClient, NetMQMessageBusClient>();
        services.AddSingleton<IActiveSessionManager, ActiveSessionManager>();
        services.Configure<NetMQMessageBusClientOptions>(x =>
        {
            x.PortForSubscribers = portForSubscribers;
            x.PortForPublishers = portForPublishers;
            x.BrokerServerPort = brokerServerPort;
            x.WorkerId = clientId;
        });

        services.AddSingleton<IMessageToByteSerializer, TypedMessageToByteSerializer>();
        services.AddSingleton<IMessageHandlerManager, MessageHandlerManager>();
        services.Configure<MessageHandlerManagerOptions>(x =>
        {
            var messageHandlerTypes = builder.services
                .Where(service => GenericsHelper.IsAssignableToGenericType(service.ServiceType, typeof(IMessageHandler<>)));

            foreach (var messageHandlerService in messageHandlerTypes)
            {
                Type handlerType = messageHandlerService.ImplementationType!;
                Type messageType = GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<>))[0];
                MethodInfo handleMethodInfo = typeof(IMessageHandler<>).MakeGenericType(messageType).GetMethod(nameof(IMessageHandler<IMessage>.Handle))!;
                x.Handlers.Add(new MessageHandlerInfo(handlerType, messageType, handleMethodInfo));
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
                x.Handlers.Add(new MessageHandlerInfo(handlerType, messageType, responseType, handleWithResponseMethodInfo));
            }
        });
        return builder;
    }


}
