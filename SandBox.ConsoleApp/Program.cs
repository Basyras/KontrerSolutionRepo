﻿// See https://aka.ms/new-console-template for more information
using Basyc.MessageBus.Broker.NetMQ;
using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.NetMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using SandBox.ConsoleApp;


int portForSub = 8987;
int portForPub = 8988;



Task brokerTask = Task.Run(() =>
{
    var broker = new NetMQMessageBroker(Options.Create(new NetMQMessageBrokerOptions() { PortForPublishers = portForPub, PortForSubscribers = portForSub }), NullLoggerFactory.Instance.CreateLogger<NetMQMessageBroker>());
    broker.Start();
});

Task clientTask = Task.Run(async () =>
{
    try
    {
        IServiceCollection clientServices = new ServiceCollection();
        
        clientServices.AddLogging(x => 
        {
            x.AddDebug();
            x.AddConsole();
        });

        clientServices
            .AddMessageBusClient()
            .RegisterMessageHandlers<Program>()
            .AddNetMQProvider(portForPub, portForSub);

        var services = clientServices.BuildServiceProvider();

        var client = services.GetRequiredService<IMessageBusClient>();
        while (Console.ReadLine() != "stop")
        {
           var response = await client.RequestAsync<CreateCustomerCommand, CreateCustomerResponse>(new("Jan"));
        }
    }
    catch (Exception ex)
    {

    }

});

await Task.WhenAll(brokerTask, clientTask);


