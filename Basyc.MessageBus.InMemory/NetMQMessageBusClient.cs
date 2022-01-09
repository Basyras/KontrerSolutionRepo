using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using OneOf;

namespace Basyc.MessageBus.Client.NetMQ;

//https://zguide.zeromq.org/docs/chapter3/#A-Load-Balancing-Message-Broker
public partial class NetMQMessageBusClient : ISimpleMessageBusClient
{
    private readonly IOptions<NetMQMessageBusClientOptions> options;
    private readonly IMessageHandlerManager handlerManager;
    private readonly ILogger<NetMQMessageBusClient> logger;
    private readonly IActiveSessionManager activeSessionStorage;
    private readonly IMessageToByteSerializer messageToByteSerializer;
    private readonly NetMQPoller poller = new NetMQPoller();
    private DealerSocket dealerSocket;

    public NetMQMessageBusClient(
        IOptions<NetMQMessageBusClientOptions> options,
        IMessageHandlerManager handlerManager,
        ILogger<NetMQMessageBusClient> logger,
        IActiveSessionManager activeSessionStorage,
        IMessageToByteSerializer messageToByteSerializer)
    {
        this.options = options;
        this.handlerManager = handlerManager;
        this.logger = logger;
        this.activeSessionStorage = activeSessionStorage;
        this.messageToByteSerializer = messageToByteSerializer;

        dealerSocket = new DealerSocket($"tcp://{options.Value.BrokerServerAddress}:{options.Value.BrokerServerPort}");
        if (options.Value.WorkerId is null)
        {
            options.Value.WorkerId = "Client-" + Guid.NewGuid();
        }

        var clientIdBytes = Encoding.ASCII.GetBytes(options.Value.WorkerId);
        dealerSocket.Options.Identity = clientIdBytes;

        dealerSocket.ReceiveReady += async (s, a) =>
        {
            await DealerHandleMessage(CancellationToken.None);
        };

        poller.Add(dealerSocket);
    }



    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        poller.RunAsync();
        CheckInMessage checkIn = new CheckInMessage(options.Value.WorkerId!, handlerManager.GetConsumableMessageTypes());
        var seriMessage = messageToByteSerializer.Serialize(checkIn, TypedToSimpleConverter.ConvertTypeToSimple(typeof(CheckInMessage)), default, MessageCase.CheckIn);

        var messageToServer = new NetMQMessage();
        messageToServer.AppendEmptyFrame();
        messageToServer.Append(seriMessage);
        logger.LogInformation($"Sending CheckIn message");
        dealerSocket.SendMultipartMessage(messageToServer);
        logger.LogInformation($"CheckIn message sent");
        return Task.CompletedTask;
    }

    private async Task DealerHandleMessage(CancellationToken cancellationToken)
    {
        var messageFrames = dealerSocket.ReceiveMultipartMessage(3);
        var senderAddressBytes = messageFrames[1].Buffer;
        var senderAddressString = Encoding.ASCII.GetString(senderAddressBytes);
        byte[] messageDataBytes = messageFrames[3].Buffer;

        var deserializationResult = messageToByteSerializer.Deserialize(messageDataBytes);
        deserializationResult.Switch(
            checkIn => logger.LogError("Client is not supposed to receive CheckIn messages"),
            async request =>
            {
                logger.LogDebug($"Request received from {senderAddressString}:{request.SessionId}, data: '{request.RequestData}'");
                object responseData = await handlerManager.ConsumeMessage(request.RequestType, request.RequestData, cancellationToken);
             
                var responseType = TypedToSimpleConverter.ConvertTypeToSimple(responseData.GetType());
                byte[] responseBytes = messageToByteSerializer.Serialize(responseData, responseType, request.SessionId, MessageCase.Response);
                var messageToServer = new NetMQMessage();
                messageToServer.AppendEmptyFrame();
                messageToServer.Append(responseBytes);
                messageToServer.AppendEmptyFrame();
                messageToServer.Append(senderAddressBytes);

                logger.LogInformation($"Sending response message");
                dealerSocket.SendMultipartMessage(messageToServer);
                logger.LogInformation($"Response message sent");
            },
            response =>
            {
                logger.LogInformation($"Response received from {senderAddressString}:{response.SessionId}, data: {response.ReponseData}");
                if (activeSessionStorage.TryCompleteSession(response.SessionId, response.ReponseData) is false)
                    logger.LogError($"Session '{response.SessionId}' completation failed. Session does not exist");
            },
            async @event =>
            {
                logger.LogInformation($"Event received from {senderAddressString}:{@event.SessionId}, data: '{@event.EventData}'");
                var responseData = await handlerManager.ConsumeMessage(@event.EventType, @event.EventData, cancellationToken);                
            },
            failure =>
            {
                logger.LogError($"Failure received from {senderAddressString}:{failure.SessionId}, data: '{failure}'");
                switch (failure.MessageCase)
                {
                    case MessageCase.Response:
                        if (activeSessionStorage.TryCompleteSession(failure.SessionId, new ErrorMessage(failure.ErrorMessage)) is false)
                            logger.LogCritical($"Session '{failure.SessionId}' failed. Session does not exist");
                        break;
                    default:
                        throw new NotImplementedException();
                }
            });

    }
    private Task PublishAsync(object? eventData, string eventType, CancellationToken cancellationToken)
    {
        Task<object> task = Task.Run(() =>
        {           
            cancellationToken.ThrowIfCancellationRequested();
            var newSession = activeSessionStorage.CreateSession(eventType);
            var serializedMessage = messageToByteSerializer.Serialize(eventData, eventType, newSession.SessionId, MessageCase.Event);
            var messageToBroker = new NetMQMessage();
            messageToBroker.AppendEmptyFrame();
            messageToBroker.Append(serializedMessage);

            cancellationToken.ThrowIfCancellationRequested();

            logger.LogInformation($"Publishing '{eventType}' session: '{newSession.SessionId}'");
            try
            {
                dealerSocket.SendMultipartMessage(messageToBroker);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to send request");
                activeSessionStorage.TryCompleteSession(newSession.SessionId, new ErrorMessage("Failed to publish"));
            }

            logger.LogInformation($"Published '{eventType}'");
            activeSessionStorage.TryCompleteSession(newSession.SessionId, "Published");            
            return newSession.ResponseSource.Task;
        });

        return task;
    }

    private Task SendAsync(object? commnadData, string commandType, CancellationToken cancellationToken)
    {
        var task = Task.Run(async () =>
        {
            await RequestAsync(commnadData, commandType, cancellationToken);
        });
        task.ConfigureAwait(false);
        return task;
    }

    private async Task<OneOf<object, ErrorMessage>> RequestAsync(object? requestData, string requestType, CancellationToken cancellationToken)
    {
        Task<object> task = Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var newSession = activeSessionStorage.CreateSession(requestType);
            var serializedMessage = messageToByteSerializer.Serialize(requestData, requestType, newSession.SessionId, MessageCase.Request);
            var messageToBroker = new NetMQMessage();
            messageToBroker.AppendEmptyFrame();
            messageToBroker.Append(serializedMessage);

            cancellationToken.ThrowIfCancellationRequested();

            logger.LogInformation($"Requesting '{requestType}'");
            try
            {
                dealerSocket.SendMultipartMessage(messageToBroker);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to send request");
                activeSessionStorage.TryCompleteSession(newSession.SessionId, new ErrorMessage("Failed to send request"));
            }

            logger.LogInformation($"Requested '{requestType}'");
            return newSession.ResponseSource.Task;
        });

        return await task;
    }
    Task ISimpleMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
    {
        return PublishAsync(null, eventType, cancellationToken);
    }

    Task ISimpleMessageBusClient.PublishAsync(string eventType, object eventData, CancellationToken cancellationToken)
    {
        return PublishAsync(eventData, eventType, cancellationToken);
    }

    Task ISimpleMessageBusClient.SendAsync(string commandType, CancellationToken cancellationToken)
    {
        return SendAsync(null, commandType, cancellationToken);
    }

    Task ISimpleMessageBusClient.SendAsync(string commandType, object commandData, CancellationToken cancellationToken)
    {
        return SendAsync(commandData, commandType, cancellationToken);
    }

    async Task<object> ISimpleMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
    {
        return await RequestAsync(null, requestType, cancellationToken);
    }

    async Task<OneOf<object, ErrorMessage>> ISimpleMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
    {
        return await RequestAsync(requestData, requestType, cancellationToken);
    }

    public void Dispose()
    {
        dealerSocket.Dispose();
        poller.Dispose();
    }

}
