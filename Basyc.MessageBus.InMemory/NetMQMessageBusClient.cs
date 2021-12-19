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

public class NetMQMessageBusClient : IMessageBusClient
{
    private readonly IServiceProvider serviceProvider;
    private readonly IOptions<NetMQMessageBusClientOptions> options;
    private readonly ILogger<NetMQMessageBusClient> logger;
    private readonly NetMQPoller poller = new NetMQPoller();
    private readonly PublisherSocket publisherSocket;
    private readonly PushSocket pushSocket;
    private readonly PullSocket? pullSocket;
    private SubscriberSocket? subscriberSocket;
    private DealerSocket dealerSocket;
    private Dictionary<int, Session> sessions = new Dictionary<int, Session>();
    private int lastUsedSessionId = 1;
    private bool disposedValue;

    public NetMQMessageBusClient(IServiceProvider serviceProvider, IOptions<NetMQMessageBusClientOptions> options, ILogger<NetMQMessageBusClient> logger)
    {
        this.serviceProvider = serviceProvider;
        this.options = options;
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

        //pushSocket = new PushSocket($"@tcp://*:{options.Value.PortForPull}");
        //poller.Add(pushSocket);

        if (options.Value.IsConsumerOfMessages)
        {
            pullSocket = new PullSocket($">tcp://localhost:{options.Value.PortForPull}");
            pullSocket.ReceiveReady += async (s, a) =>
            {
                await PullWaitAndHandleNextMessage();
            };
            poller.Add(pullSocket);
        }

        dealerSocket = new DealerSocket($"tcp://localhost:{options.Value.BrokerServerPort}");
        if (options.Value.ClientId is not null)
        {
            //dealerSocket.Options.Identity = Encoding.Unicode.GetBytes("BusClient");
            dealerSocket.Options.Identity = Encoding.Unicode.GetBytes(options.Value.ClientId);
        }

        
        dealerSocket.ReceiveReady += async (s, a) =>
        {
            await DealerHandleResponseMessage();
        };
        poller.Add(dealerSocket);
    }



    public void StartAsync()
    {
        poller.RunAsync("netmqpollerthread", true);
        string identity = Encoding.Unicode.GetString(dealerSocket.Options.Identity!);
        CheckInMessage checkIn = new CheckInMessage(identity,0);        
        var seriMessage = MessageSerializer.Serialize(checkIn, default, false);

        var messageToServer = new NetMQMessage();
        messageToServer.AppendEmptyFrame();
        messageToServer.Append(seriMessage);
        logger.LogInformation($"Sending CheckIn message");
        dealerSocket.SendMultipartMessage(messageToServer);
        logger.LogInformation($"CheckIn message sent");
    }

    private Task DealerHandleResponseMessage()
    {
        logger.LogDebug($"Dealer waiting");
        var message = dealerSocket.ReceiveMultipartMessage(3);
        logger.LogDebug($"Dealer received");
        var clientAddress = message[1].ConvertToString();
        DeserializedMessageResult deserializedMessageResult = MessageSerializer.DeserializeMessage(message[3].Buffer);
        var messageToServer = new NetMQMessage();
        messageToServer.AppendEmptyFrame();
        messageToServer.Append(message[3].Buffer);
        logger.LogInformation($"Sending response message");
        dealerSocket.SendMultipartMessage(messageToServer);
        logger.LogInformation($"Response message sent");

        return Task.CompletedTask;
      
    }
    private async Task PullWaitAndHandleNextMessage()
    {
        logger.LogDebug($"Waiting for message");

        //string topic = subscriberSocket!.ReceiveFrameString();
        //byte[] messageBytes = subscriberSocket!.ReceiveFrameBytes();
        //
        byte[] messageBytes = pullSocket!.ReceiveFrameBytes();

        DeserializedMessageResult deserializedMessageResult = MessageSerializer.DeserializeMessage(messageBytes);
        if (deserializedMessageResult.IsResponse)
        {
            if (sessions.TryGetValue(deserializedMessageResult.SessionId, out var session) is false)
            {
                logger.LogDebug($"This bus client does not have active seesion for this message");
                return;
            }

            logger.LogDebug($"Response recieved");
            if (deserializedMessageResult.Message is VoidResult voidResult)
            {
                sessions[deserializedMessageResult.SessionId].responseSource.SetResult(voidResult);
            }
            else
            {
                sessions[deserializedMessageResult.SessionId].responseSource.SetResult(deserializedMessageResult.Message);
            }
            sessions.Remove(deserializedMessageResult.SessionId);
            logger.LogDebug($"Session completed");

        }
        else
        {
            MessageHandlerInfo? handlerInfo = options.Value.Handlers.FirstOrDefault(x => x.MessageType == deserializedMessageResult.MessageType);
            if (handlerInfo is null)
            {
                logger.LogDebug($"This bus client does not have handler for this message");
                return;
            }

            if (deserializedMessageResult.ExpectsResponse)
            {
                Type proxyConsumerType = typeof(IMessageHandler<,>).MakeGenericType(deserializedMessageResult.MessageType, deserializedMessageResult.ResponseType!);

                object handlerInstace = serviceProvider.GetRequiredService(proxyConsumerType)!;
                Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { deserializedMessageResult.Message, CancellationToken.None })!;
                await handlerResult;
                object taskResult = ((dynamic)handlerResult).Result!;
                //await PublishAsync(taskResult, taskResult.GetType(), default, true, deserializedMessageResult.SessionId);
                await Send(taskResult, taskResult.GetType(), default, deserializedMessageResult.SessionId);

            }
            else
            {
                Type proxyConsumerType = typeof(IMessageHandler<>).MakeGenericType(deserializedMessageResult.MessageType);
                object handlerInstace = serviceProvider.GetRequiredService(proxyConsumerType)!;
                Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { deserializedMessageResult.Message, CancellationToken.None })!;
                //await PublishAsync(new VoidResult(), typeof(VoidResult), default, true, deserializedMessageResult.SessionId);
                await Send(new VoidResult(), typeof(VoidResult), default, deserializedMessageResult.SessionId);
            }

        }
    }

    Task IMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
    {
        return PublishAsync(default(TEvent), cancellationToken);
    }

    Task IMessageBusClient.PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken)
    {
        return PublishAsync(data, cancellationToken);
    }

    private Task PublishAsync<TEvent>(TEvent? eventMessage, CancellationToken cancellationToken, bool isResponse = false) where TEvent : IEventMessage
    {
        return PublishAsync(eventMessage, typeof(TEvent), cancellationToken, isResponse);
    }
    private Task PublishAsync(object? eventMessage, Type eventType, CancellationToken cancellationToken, bool isResponse = false, int sessionId = default)
    {
        if (sessionId is default(int))
            sessionId = ++lastUsedSessionId;

        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        if (eventMessage is null)
            eventMessage = Activator.CreateInstance(eventType);



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
            logger.LogInformation($"Publishing {eventType} event");

            if (cancellationToken.IsCancellationRequested)
                return;

            byte[] serializedEvent = MessageSerializer.Serialize(eventMessage!, sessionId, isResponse)!;
            publisherSocket.SendMoreFrame(eventType.FullName!).SendFrame(serializedEvent);
            //pushSocket.SendFrame(serializedEvent);
            logger.LogInformation($"Published {eventType} event");
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

    private Task Send(object? command, Type commandType, CancellationToken cancellationToken, int sessionId = default)
    {
        if (sessionId is default(int))
            sessionId = ++lastUsedSessionId;

        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        if (command is null)
            command = Activator.CreateInstance(commandType);


        var task = Task.Run(() =>
        {
            logger.LogInformation($"Sending {commandType} request");

            if (cancellationToken.IsCancellationRequested)
                return;

            var data = MessageSerializer.Serialize(command!, sessionId);
            //publisherSocket.SendMoreFrame(commandType.FullName!).SendFrame(data);
            pushSocket.SendFrame(data);
            //TaskCompletionSource<object?> responseSource = new TaskCompletionSource<object?>();
            //sessions.Add(sessionId, new Session(sessionId, null, responseSource));

            logger.LogInformation($"Sent {commandType} request");
        });

        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;



        //using var runtime = new NetMQRuntime();
        //runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;


        return task;
    }

    private Task<object?> Request(object? request, Type requestType, Type responseType, CancellationToken cancellationToken)
    {
        int newSessionId = ++lastUsedSessionId;
        cancellationToken.ThrowIfCancellationRequested();

        if (request is null)
            request = Activator.CreateInstance(requestType);


        Task<object?> task = Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
        
            var serializedMessage = MessageSerializer.Serialize(request!, newSessionId);
            //publisherSocket.SendMoreFrame("*").SendFrame(MessageSerializer.SerializeCommand(request!, newSessionId));
            //pushSocket.SendFrame(MessageSerializer.SerializeCommand(request!, newSessionId));
            var messageToServer = new NetMQMessage();
            messageToServer.AppendEmptyFrame();
            messageToServer.Append(serializedMessage);
            logger.LogInformation($"Requesting {requestType}");
            dealerSocket.SendMultipartMessage(messageToServer);
            logger.LogInformation($"Requested {requestType}");
            TaskCompletionSource<object?> responseSource = new TaskCompletionSource<object?>();
            sessions.Add(newSessionId, new Session(newSessionId, responseType, responseSource));
            logger.LogInformation($"Waiting for request response {requestType}");
            return responseSource.Task;
        });

        cancellationToken.ThrowIfCancellationRequested();
        //using NetMQRuntime runtime = new NetMQRuntime();
        //runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        cancellationToken.ThrowIfCancellationRequested();
        return task;
    }

    Task IMessageBusClient.SendAsync<TCommand>(CancellationToken cancellationToken)
    {
        return Send(default(TCommand), typeof(TCommand), cancellationToken);
    }

    Task IMessageBusClient.SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
    {
        return Send(command, typeof(TCommand), cancellationToken);
    }

    Task IMessageBusClient.SendAsync(Type commandType, object command, CancellationToken cancellationToken)
    {
        return Send(command, commandType, cancellationToken);
    }

    Task IMessageBusClient.SendAsync(Type commandType, CancellationToken cancellationToken)
    {
        return Send(Activator.CreateInstance(commandType), commandType, cancellationToken);
    }

    async Task<TResponse> IMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
    {
        return (TResponse)(await Request(null, typeof(TRequest), typeof(TResponse), cancellationToken))!;
    }

    Task<object> IMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
    {
        return (Request(null, requestType, responseType, cancellationToken))!;
    }

    Task<object> IMessageBusClient.RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken)
    {
        return (Request(request, requestType, responseType, cancellationToken))!;
    }

    async Task<TResponse> IMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        return (TResponse)(await Request(request, typeof(TRequest), typeof(TResponse), cancellationToken))!;
    }




    private record Session(int SessionId, Type? ResponseType, TaskCompletionSource<object?> responseSource);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                publisherSocket?.Dispose();
                subscriberSocket?.Dispose();
                pushSocket?.Dispose();
                pullSocket?.Dispose();
                poller.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~NetMQMessageBusClient()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
