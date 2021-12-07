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
    private SubscriberSocket? subscriberSocket;
    private Dictionary<int, Session> sessions = new Dictionary<int, Session>();
    private int lastUsedSessionId = 1;

    public NetMQMessageBusClient(IServiceProvider serviceProvider, IOptions<NetMQMessageBusClientOptions> options, ILogger<NetMQMessageBusClient> logger)
    {
        this.serviceProvider = serviceProvider;
        this.options = options;
        this.logger = logger;
        publisherSocker = new PublisherSocket($">tcp://127.0.0.1:{options.Value.PortForPublishers}");

        if (options.Value.IsConsumerOfMessages)
        {
            Task.Run(async () =>
            {
                subscriberSocket = new SubscriberSocket($">tcp://127.0.0.1:{options.Value.PortForSubscribers}");
                subscriberSocket.SubscribeToAnyTopic();
                while (true)
                {
                    try
                    {
                        await HandleResponse();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            });
        }

        //Task.Run(async () =>
        //{
        //    subscriberSocket = new SubscriberSocket($">tcp://127.0.0.1:{options.Value.PortForSubscribers}");
        //    subscriberSocket.Subscribe("TEST");
        //    while (true)
        //    {
        //        try
        //        {
        //             //sessions[deserializedMessageResult.SessionId].responseSource.SetResult(taskResult);
        //             //sessions[deserializedMessageResult.SessionId].responseSource.SetResult(null);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //});
    }

    private async Task HandleResponse()
    {
        logger.LogInformation($"Waiting for message");
        string topic = subscriberSocket!.ReceiveFrameString();
        byte[] messageBytes = subscriberSocket!.ReceiveFrameBytes();
        DeserializedMessageResult deserializedMessageResult = MessageSerializer.DeserializeMessage(messageBytes);
        if (deserializedMessageResult.IsResponse)
        {
            logger.LogInformation($"Response recieved");
            if (sessions.TryGetValue(deserializedMessageResult.SessionId, out var session))
            {
                if (deserializedMessageResult.Message is VoidResult)
                {
                    sessions[deserializedMessageResult.SessionId].responseSource.SetResult(null);
                }
                else
                {
                    sessions[deserializedMessageResult.SessionId].responseSource.SetResult(deserializedMessageResult.Message);
                }
                sessions.Remove(deserializedMessageResult.SessionId);
                logger.LogInformation($"Session completed");
            }
            else
            {
                logger.LogInformation($"Response recieved with unknown sessionId");
            }
        }
        else
        {
            MessageHandlerInfo handlerInfo = options.Value.Handlers.First(x => x.MessageType == deserializedMessageResult.MessageType);
            if (deserializedMessageResult.ExpectsResponse)
            {
                Type proxyConsumerType = typeof(IMessageHandler<,>).MakeGenericType(deserializedMessageResult.MessageType, deserializedMessageResult.ResponseType!);

                object handlerInstace = serviceProvider.GetRequiredService(proxyConsumerType)!;
                Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { deserializedMessageResult.Message, CancellationToken.None })!;
                await handlerResult;
                object taskResult = ((dynamic)handlerResult).Result!;
                await PublishAsync(taskResult, taskResult.GetType(), default, true, deserializedMessageResult.SessionId);

            }
            else
            {
                Type proxyConsumerType = typeof(IMessageHandler<>).MakeGenericType(deserializedMessageResult.MessageType);
                object handlerInstace = serviceProvider.GetRequiredService(proxyConsumerType)!;
                Task handlerResult = (Task)handlerInfo.HandleMethodInfo.Invoke(handlerInstace, new object[] { deserializedMessageResult.Message, CancellationToken.None })!;
                await PublishAsync(new VoidResult(), typeof(VoidResult), default, true, deserializedMessageResult.SessionId);
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

        logger.LogInformation($"Publishing {eventType} event");
        var task = Task.Run(() =>
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            byte[] serializedEvent = MessageSerializer.SerializeCommand(eventMessage!, sessionId, isResponse)!;
            publisherSocker.SendMoreFrame(eventType.FullName!).SendFrame(serializedEvent);
        });

        using var runtime = new NetMQRuntime();
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        logger.LogInformation($"Published {eventType} event");
        return Task.CompletedTask;
    }

    private Task Send(object? command, Type commandType, CancellationToken cancellationToken)
    {
        int newSessionId = ++lastUsedSessionId;
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        if (command is null)
            command = Activator.CreateInstance(commandType);

        logger.LogInformation($"Sending {commandType} request");
        var task = Task.Run(() =>
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var data = MessageSerializer.SerializeCommand(command!, newSessionId);
            publisherSocker.SendMoreFrame(commandType.FullName!).SendFrame(data);
        });

        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        TaskCompletionSource<object?> responseSource = new TaskCompletionSource<object?>();
        sessions.Add(newSessionId, new Session(newSessionId, null, responseSource));

        using var runtime = new NetMQRuntime();
        runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        logger.LogInformation($"Sent {commandType} request");
        return responseSource.Task;
    }

    private Task<object?> Request(object? request, Type requestType, Type responseType, CancellationToken cancellationToken)
    {
        int newSessionId = ++lastUsedSessionId;
        cancellationToken.ThrowIfCancellationRequested();

        if (request is null)
            request = Activator.CreateInstance(requestType);

        logger.LogInformation($"Sending {requestType} request");
        Task task = Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            publisherSocker.SendMoreFrame("*").SendFrame(MessageSerializer.SerializeCommand(request!, newSessionId));
        });

        using NetMQRuntime runtime = new NetMQRuntime();
        cancellationToken.ThrowIfCancellationRequested();
        TaskCompletionSource<object?> responseSource = new TaskCompletionSource<object?>();
        sessions.Add(newSessionId, new Session(newSessionId, responseType, responseSource));

        runtime.Run(task);

        if (task.Exception is not null)
            throw task.Exception;

        cancellationToken.ThrowIfCancellationRequested();



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
    private record Session(int SessionId, Type? ResponseType, TaskCompletionSource<object?> responseSource);



}
