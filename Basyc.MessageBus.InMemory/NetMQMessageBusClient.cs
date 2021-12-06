using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Basyc.MessageBus.Client.RequestResponse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Basyc.MessageBus.Client.NetMQ;

public class NetMQMessageBusClient : IMessageBusClient, IDisposable
{
    private readonly IServiceProvider serviceProvider;
    private readonly IOptions<NetMQMessageBusClientOptions> options;
    private readonly ILogger<NetMQMessageBusClient> logger;
    private readonly PublisherSocket publisherSocker;
    private readonly SubscriberSocket? subscriberSocket;
    private Dictionary<int, Session> sessions = new Dictionary<int, Session>();
    private int lastUsedSessionId = 1;

    public NetMQMessageBusClient(IServiceProvider serviceProvider, IOptions<NetMQMessageBusClientOptions> options, ILogger<NetMQMessageBusClient> logger)
    {
        this.serviceProvider = serviceProvider;
        this.options = options;
        this.logger = logger;
        publisherSocker = new PublisherSocket("tcp://localhost:5555");

        if (options.Value.IsConsumerOfMessages)
        {
            subscriberSocket = new SubscriberSocket("tcp://localhost:5555");
            subscriberSocket.SubscribeToAnyTopic();
            Task.Run(() =>
            {
                while (true)
                {
                    HandleResponse();
                }
            });
        }
    }

    private void HandleResponse()
    {
        string topic = subscriberSocket!.ReceiveFrameString();
        byte[] messageBytes = subscriberSocket!.ReceiveFrameBytes();
        DeserializedMessageResult deserializedMessageResult = MessageSerializer.DeserializeMessage(messageBytes);
        MessageHandlerInfo handlerInfo = options.Value.Handlers.First(x => x.MessageType == deserializedMessageResult.MessageType);
        object handlerInstace = serviceProvider.GetRequiredService(handlerInfo.HandlerType)!;
        object? handlerResult = handlerInfo.HandleMethod.Invoke(handlerInstace, new object[] { deserializedMessageResult.Message });
        if (deserializedMessageResult.ExpectsResponse)
        {            
            sessions[deserializedMessageResult.SessionId].responseSource.SetResult(handlerResult!);
        }
        else
        {
            sessions[deserializedMessageResult.SessionId].responseSource.SetResult(null);
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

    private Task PublishAsync<TEvent>(TEvent? eventMessage, CancellationToken cancellationToken) where TEvent : IEventMessage
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        if (eventMessage is null)
            eventMessage = Activator.CreateInstance<TEvent>();

        logger.LogInformation($"Publishing {typeof(TEvent)} event");
        var task = Task.Run(() =>
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            byte[] serializedEvent = MessageSerializer.SerializeCommand(eventMessage, ++lastUsedSessionId)!;
            publisherSocker.SendMoreFrame("*").SendFrame(serializedEvent);
        });

        using var runtime = new NetMQRuntime();
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        logger.LogInformation($"Published {typeof(TEvent)} event");
        return Task.CompletedTask;
    }

    private Task Send(object? command, Type commandType, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        if (command is null)
            command = Activator.CreateInstance(commandType);

        logger.LogInformation($"Sending {commandType} request");
        var task = Task.Run(() =>
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            publisherSocker.SendMoreFrame("*").SendFrame(MessageSerializer.SerializeCommand(command!, ++lastUsedSessionId));
        });

        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        using var runtime = new NetMQRuntime();
        runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        logger.LogInformation($"Sent {commandType} request");
        return task;
    }

    private Task<object?> Request(object? request, Type requestType, Type responseType, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request is null)
            request = Activator.CreateInstance(requestType);
        int newSessionId = ++lastUsedSessionId;
        logger.LogInformation($"Sending {requestType} request");
        Task task = Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            publisherSocker.SendMoreFrame("*").SendFrame(MessageSerializer.SerializeCommand(request!, newSessionId));
        });

        using NetMQRuntime runtime = new NetMQRuntime();
        cancellationToken.ThrowIfCancellationRequested();

        runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        cancellationToken.ThrowIfCancellationRequested();
        TaskCompletionSource<object?> responseSource = new TaskCompletionSource<object?>();
        sessions.Add(newSessionId, new Session(newSessionId, responseType, responseSource));


        logger.LogInformation($"Send finished {requestType} request");
        return responseSource.Task;
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



    public void Dispose()
    {
        publisherSocker.Dispose();
        subscriberSocket?.Dispose();
    }
    private record Session(int CommunicationId, Type ResponseType, TaskCompletionSource<object?> responseSource);



}
