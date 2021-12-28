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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Basyc.MessageBus.Client.NetMQ;

//https://zguide.zeromq.org/docs/chapter3/#A-Load-Balancing-Message-Broker
public partial class NetMQMessageBusClient : ISimpleMessageBusClient
{
    private readonly IServiceProvider serviceProvider;
    private readonly IOptions<NetMQMessageBusClientOptions> options;
    private readonly IMessageHandlerManager handlerManager;
    private readonly ILogger<NetMQMessageBusClient> logger;
    private readonly IActiveSessionManager activeSessionStorage;
    private readonly NetMQPoller poller = new NetMQPoller();
    private readonly PublisherSocket publisherSocket;
    private SubscriberSocket? subscriberSocket;
    private DealerSocket dealerSocket;
    //private Dictionary<int, ActiveSession> sessions = new Dictionary<int, ActiveSession>();
    private int lastUsedSessionId = 1;

    public NetMQMessageBusClient(IServiceProvider serviceProvider, 
        IOptions<NetMQMessageBusClientOptions> options, 
        IMessageHandlerManager handlerManager, 
        ILogger<NetMQMessageBusClient> logger,
        IActiveSessionManager activeSessionStorage)
    {
        this.serviceProvider = serviceProvider;
        this.options = options;
        this.handlerManager = handlerManager;
        this.logger = logger;
        this.activeSessionStorage = activeSessionStorage;

        //publisherSocket = new PublisherSocket($">tcp://localhost:{options.Value.PortForPublishers}");
        //poller.Add(publisherSocket);

        //subscriberSocket = new SubscriberSocket($">tcp://localhost:{options.Value.PortForSubscribers}");
        //subscriberSocket.SubscribeToAnyTopic();
        //subscriberSocket.ReceiveReady += async (s, a) =>
        //{
        //    await PullWaitAndHandleNextMessage();
        //};
        //poller.Add(subscriberSocket);

        dealerSocket = new DealerSocket($"tcp://localhost:{options.Value.BrokerServerPort}");
        if (options.Value.WorkerId is null)
        {
            options.Value.WorkerId = "Client-" + Guid.NewGuid();
        }

        var clientIdBytes = Encoding.ASCII.GetBytes(options.Value.WorkerId);
        dealerSocket.Options.Identity = clientIdBytes;

        dealerSocket.ReceiveReady += async (s, a) =>
        {
            await DealerHandleMessage();
        };

        poller.Add(dealerSocket);
    }



    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        poller.RunAsync();
        CheckInMessage checkIn = new CheckInMessage(options.Value.WorkerId!, handlerManager.GetConsumableMessageTypes());
        var seriMessage = TypedMessageToByteSerializer.Serialize(checkIn, default, false);

        var messageToServer = new NetMQMessage();
        messageToServer.AppendEmptyFrame();
        messageToServer.Append(seriMessage);
        logger.LogInformation($"Sending CheckIn message");
        dealerSocket.SendMultipartMessage(messageToServer);
        logger.LogInformation($"CheckIn message sent");
        return Task.CompletedTask;
    }

    private async Task DealerHandleMessage()
    {
        var messageFrames = dealerSocket.ReceiveMultipartMessage(3);
        var producerAddressBytes = messageFrames[1].Buffer;
        var producerAddressString = Encoding.ASCII.GetString(producerAddressBytes);
        byte[] messageDataBytes = messageFrames[3].Buffer;
        DeserializedMessage deserializedMessage = TypedMessageToByteSerializer.Deserialize(messageDataBytes);
        switch (deserializedMessage.MessageCase)
        {
            case MessageCase.Request:
                var request = deserializedMessage.Request!;
                logger.LogDebug($"Request recieved from {producerAddressString}");
                var handlerResult = await handlerManager.ConsumeMessage(request.RequestType, request.RequestData);
                byte[] responseBytes = TypedMessageToByteSerializer.Serialize(handlerResult, request.SessionId, true);
                var messageToServer = new NetMQMessage();
                messageToServer.AppendEmptyFrame();
                messageToServer.Append(responseBytes);
                messageToServer.AppendEmptyFrame();
                messageToServer.Append(producerAddressBytes);

                logger.LogInformation($"Sending response message");
                dealerSocket.SendMultipartMessage(messageToServer);
                logger.LogInformation($"Response message sent");
                break;

            case MessageCase.Response:
                var response = deserializedMessage.Response!;

                if (response.ReponseData is FailResult failure)
                {
                    logger.LogError($"Failure recieved: {failure.Message}");

                    if (response.SessionId == ActiveSession.UnknownSessionId)
                    {
                       logger.LogCritical($"Cannot finish session, SessionId not recieved");                       
                    }
                    else
                    {
                        if (activeSessionStorage.TryCompleteSession(response.SessionId, response.ReponseData))
                            logger.LogDebug($"Session '{response.SessionId}' completed");
                        else
                            logger.LogError($"Session '{response.SessionId}' failed. Session does not exist");
                    }
                    return;
                }


                logger.LogInformation($"Response recieved from {producerAddressString}, message: {response.ReponseData}");
                if(activeSessionStorage.TryCompleteSession(response.SessionId, response.ReponseData))
                    logger.LogDebug($"Session '{response.SessionId}' completed");
                else
                    logger.LogError($"Session '{response.SessionId}' failed. Session does not exist");

                break;
        }

    }
    private Task PublishAsync(object? eventData, string eventType, CancellationToken cancellationToken, bool isResponse = false, int sessionId = default)
    {
        if (sessionId is default(int))
            sessionId = ++lastUsedSessionId;

        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;



        //Task task = new Task(() =>
        //{
        //    if (cancellationToken.IsCancellationRequested)
        //        return;

        //    byte[] serializedEvent = MessageSerializer.SerializeCommand(eventMessage!, sessionId, isResponse)!;
        //    publisherSocket.SendMoreFrame(eventType.FullName!).SendFrame(serializedEvent);
        //});

        //Task task = Task.Run(() =>
        //{
        //    if (cancellationToken.IsCancellationRequested)
        //        return;

        //    byte[] serializedEvent = MessageSerializer.SerializeCommand(eventMessage!, sessionId, isResponse)!;
        //    publisherSocker.SendMoreFrame(eventType.FullName!).SendFrame(serializedEvent);
        //});

        var task = Task.Run(() =>
        {
            logger.LogInformation($"Publishing event: '{eventType}'");

            if (cancellationToken.IsCancellationRequested)
                return;

            byte[] serializedEvent = TypedMessageToByteSerializer.Serialize(eventData!, sessionId, isResponse)!;
            publisherSocket.SendMoreFrame(eventType)
            .SendFrame(serializedEvent);
            //pushSocket.SendFrame(serializedEvent);
            logger.LogInformation($"Published event: '{eventType}'");
        });

        using var runtime = new NetMQRuntime();
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        //runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;


        //return Task.CompletedTask;
        return task;
    }

    private Task SendAsync(object? command, string commandType, CancellationToken cancellationToken)
    {
        var task = Task.Run(async () =>
        {
            await RequestAsync(command, commandType, cancellationToken);
        });
        task.ConfigureAwait(false);
        return task;
    }

    private Task<object> RequestAsync(object? request, string requestType, CancellationToken cancellationToken)
    {
        Task<object> task = Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var newSession = activeSessionStorage.CreateSession();
            logger.LogDebug($"Session '{newSession.SessionId}' created");
            var requestCLRType = TypedToSimpleConverter.ConvertSimpleToType(requestType);
            var serializedMessage = TypedMessageToByteSerializer.Serialize(request, requestCLRType, newSession.SessionId);
            var messageToBroker = new NetMQMessage();
            messageToBroker.AppendEmptyFrame();
            messageToBroker.Append(serializedMessage);

            cancellationToken.ThrowIfCancellationRequested();

            logger.LogInformation($"Requesting '{requestType}'");
            try
            {
                dealerSocket.SendMultipartMessage(messageToBroker);
            }
            catch(Exception ex)
            {
                logger.LogCritical(ex, "Failed to send request");
                activeSessionStorage.TryCompleteSession(newSession.SessionId, new FailResult("Failed to send request"));                
            }
            
            logger.LogInformation($"Requested '{requestType}'");         
            return newSession.ResponseSource.Task;
        });

        if (task.Exception is not null)
            throw task.Exception;

        return task;
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

    Task<object> ISimpleMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
    {
        return RequestAsync(null, requestType, cancellationToken);
    }

    Task<object> ISimpleMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
    {
        return RequestAsync(requestData, requestType, cancellationToken);
    }

    public void Dispose()
    {
        publisherSocket?.Dispose();
        subscriberSocket?.Dispose();
        dealerSocket.Dispose();
        poller.Dispose();
    }

}
