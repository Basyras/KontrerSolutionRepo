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
    private readonly NetMQPoller poller = new NetMQPoller();
    private readonly PublisherSocket publisherSocket;
    private SubscriberSocket? subscriberSocket;
    private DealerSocket dealerSocket;
    private Dictionary<int, Session> sessions = new Dictionary<int, Session>();
    private int lastUsedSessionId = 1;

    public NetMQMessageBusClient(IServiceProvider serviceProvider, IOptions<NetMQMessageBusClientOptions> options, IMessageHandlerManager handlerManager, ILogger<NetMQMessageBusClient> logger)
    {
        this.serviceProvider = serviceProvider;
        this.options = options;
        this.handlerManager = handlerManager;
        this.logger = logger;

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
                logger.LogInformation($"Request recieved from {producerAddressString}");
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
                logger.LogInformation($"Response recieved from {producerAddressString}, message: {response.ReponseData}");
                if (sessions.TryGetValue(response.SessionId, out var session) is false)
                {
                    logger.LogError($"This bus client does not have active seesion for this message");
                    return;
                }


                if (response.ReponseData is FailResult failure)
                {
                    logger.LogError($"Broker responded with error: {failure.Message}");
                }

                sessions[response.SessionId].ResponseSource.SetResult(response.ReponseData);
                sessions.Remove(response.SessionId);

                logger.LogDebug($"Session '{session.SessionId}' completed");
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
        int newSessionId = ++lastUsedSessionId;

        Task<object> task = Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var requestCLRType = TypedToSimpleConverter.ConvertSimpleToType(requestType);
            //var requestCLRType = Assembly.GetEntryAssembly()!.GetType(requestType)!;
            var serializedMessage = TypedMessageToByteSerializer.Serialize(request, requestCLRType, newSessionId);
            var messageToBroker = new NetMQMessage();
            messageToBroker.AppendEmptyFrame();
            messageToBroker.Append(serializedMessage);

            cancellationToken.ThrowIfCancellationRequested();

            logger.LogInformation($"Requesting '{requestType}'");
            dealerSocket.SendMultipartMessage(messageToBroker);
            logger.LogInformation($"Requested '{requestType}'");

            TaskCompletionSource<object> responseSource = new TaskCompletionSource<object>();
            logger.LogDebug($"Session '{newSessionId}' started");
            sessions.Add(newSessionId, new Session(newSessionId, responseSource));
            logger.LogDebug($"Waiting for '{requestType}' response");
            return responseSource.Task;
        });

        if (task.Exception is not null)
            throw task.Exception;

        return task;
    }

    //Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
    //{
    //    return PublishAsync(default(TEvent), cancellationToken);
    //}

    //Task ITypedMessageBusClient.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
    //{
    //    return PublishAsync(data, cancellationToken);
    //}

    //Task ITypedMessageBusClient.SendAsync<TCommand>(CancellationToken cancellationToken)
    //{
    //    return Send(default(TCommand), typeof(TCommand), cancellationToken);
    //}

    //Task ITypedMessageBusClient.SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
    //{
    //    return Send(command, typeof(TCommand), cancellationToken);
    //}

    //Task ITypedMessageBusClient.SendAsync(Type commandType, object command, CancellationToken cancellationToken)
    //{
    //    return Send(command, commandType, cancellationToken);
    //}

    //Task ITypedMessageBusClient.SendAsync(Type commandType, CancellationToken cancellationToken)
    //{
    //    return Send(Activator.CreateInstance(commandType), commandType, cancellationToken);
    //}

    //async Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
    //{
    //    return (TResponse)(await Request(null, typeof(TRequest), typeof(TResponse), cancellationToken))!;
    //}

    //Task<object> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
    //{
    //    return (Request(null, requestType, responseType, cancellationToken))!;
    //}

    //Task<object> ITypedMessageBusClient.RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken)
    //{
    //    return (Request(request, requestType, responseType, cancellationToken))!;
    //}

    //async Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    //{
    //    return (TResponse)(await Request(request, typeof(TRequest), typeof(TResponse), cancellationToken))!;
    //}



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
        return SendAsync(null, commandType, cancellationToken);
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
