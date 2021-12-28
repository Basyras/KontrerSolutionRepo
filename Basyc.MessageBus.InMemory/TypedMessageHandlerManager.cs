using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public class TypedMessageHandlerManager : IMessageHandlerManager
    {
        private readonly IServiceProvider serviceProvider;
        private Dictionary<string, TypedMessageHandlerInfo> handlerStorage = new();
        public TypedMessageHandlerManager(IServiceProvider serviceProvider, IOptions<TypedMessageHandlerManagerOptions> options)
        {
            this.serviceProvider = serviceProvider;


            foreach (var handler in options.Value.Handlers)
            {
                handlerStorage.Add(TypedToSimpleConverter.ConvertTypeToSimple(handler.MessageType), handler);
            }
        }

        public async Task<object> ConsumeMessage(string messageType, object messageData)
        {
            if (handlerStorage.TryGetValue(messageType, out var handlerInfo))
            {
                if (handlerInfo.HasResponse)
                {
                    Type consumerType = typeof(IMessageHandler<,>).MakeGenericType(handlerInfo.MessageType, handlerInfo.ResponseType!);

                    object handlerInstace = serviceProvider.GetRequiredService(consumerType)!;
                    Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { messageData, CancellationToken.None })!;
                    await handlerResult;
                    object taskResult = ((dynamic)handlerResult).Result!;
                    return taskResult;
                }
                else
                {
                    Type consumerType = typeof(IMessageHandler<>).MakeGenericType(handlerInfo.MessageType);
                    object handlerInstace = serviceProvider.GetRequiredService(consumerType)!;
                    Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { messageData, CancellationToken.None })!;
                    await handlerResult;
                    return new VoidResult();
                }
            }

            throw new InvalidOperationException("Handler for this message not found");

        }

        public string[] GetConsumableMessageTypes()
        {
            return handlerStorage.Values.Select(x => TypedToSimpleConverter.ConvertTypeToSimple(x.MessageType)).ToArray();
        }
    }
}

