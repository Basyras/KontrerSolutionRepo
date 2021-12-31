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
using NetMQ;
using NetMQ.Sockets;
using ProtoBuf;
using SandBox.ConsoleApp;
using System.Text;

////Consumer
///
int portForSub = 8987;
int portForPub = 8988;

IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
    x.AddDebug();
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Debug);
});


clientServices
    .AddMessageBusClient()
    .RegisterTypedMessageHandlers<Program>()
    .AddNetMQClient(portForPub, portForSub, "Console1");

var services = clientServices.BuildServiceProvider();
using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();
client.StartAsync();

while (Console.ReadLine() != "stop")
{

    var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "Console12", "aasdů"))
        .GetAwaiter()
        .GetResult();

    response.Switch(x => Console.WriteLine(x),  x => Console.WriteLine(x));


}



Console.ReadLine();





