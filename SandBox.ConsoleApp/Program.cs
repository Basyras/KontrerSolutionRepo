// See https://aka.ms/new-console-template for more information
using Basyc.MessageBus.Broker;
using Basyc.MessageBus.Broker.NetMQ;
using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.NetMQ;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using ProtoBuf;
using SandBox.ConsoleApp;

int portForSub = 8987;
int portForPub = 8988;
int portForPush = 5558;
int portForPull = portForPush;
Serializer.PrepareSerializer<CreateCustomerCommandResponse>();

IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
    x.AddDebug();
    x.AddConsole();
});
clientServices.AddNetMQMessageBroker(portForSub,portForPub);

clientServices
    .AddMessageBusClient()
    .RegisterMessageHandlers<Program>()
    .AddNetMQProvider(portForPub, portForSub, portForPush, portForPull);

var services = clientServices.BuildServiceProvider();
using var broker = services.GetRequiredService<IMessageBroker>();
Task brokerTask = broker.StartAsync();

using IMessageBusClient client = services.GetRequiredService<IMessageBusClient>();
(client as NetMQMessageBusClient)!.StartAsync();

void Test()
{
    while (Console.ReadLine() != "stop")
    {
        var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "hady", "mail"))
            .GetAwaiter()
            .GetResult();
    }
}

Test();


