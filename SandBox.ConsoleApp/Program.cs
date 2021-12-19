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
int portForPull = 4558;
int portForPush = 5557;
int brokerPort = 5357;

//Serializer.PrepareSerializer<CreateCustomerCommandResponse>();
IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
    x.AddDebug();
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Debug);
});


clientServices
    .AddMessageBusClient()
    .RegisterMessageHandlers<Program>()
    .AddNetMQProvider(portForPub, portForSub, portForPush, portForPull, brokerPort,"client1");

var services = clientServices.BuildServiceProvider();
using IMessageBusClient client = services.GetRequiredService<IMessageBusClient>();
(client as NetMQMessageBusClient)!.StartAsync();


while (Console.ReadLine() != "stop")
{
    var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "Console12", "aasdů"))
        .GetAwaiter()
        .GetResult();
}



//using (var client = new RequestSocket())
//{
//    client.Connect("tcp://localhost:5555");
//    Console.WriteLine("Sending Hello");
//    client.SendFrame("Hello");
//    var message = client.ReceiveFrameString();
//    Console.WriteLine("Received {0}", message);
//}

//using var poller = new NetMQPoller();
//DealerSocket client = new DealerSocket();
//client = new DealerSocket();
//client.Options.Identity = Encoding.Unicode.GetBytes("Client-ConsoleApp1");
//client.Connect($"tcp://localhost:{brokerPort}");
//client.ReceiveReady += Client_ReceiveReady;
//poller.Add(client);
//poller.RunAsync();
//var messageToServer = new NetMQMessage();
//messageToServer.AppendEmptyFrame();
//messageToServer.Append("Client-ConsoleApp1 request1");
//Console.WriteLine("Sending to server");
//client.SendMultipartMessage(messageToServer);



//void Client_ReceiveReady(object? sender, NetMQSocketEventArgs? e)
//{
//    var response = e.Socket.ReceiveFrameString();
//    Console.WriteLine("Message from server recieved, "+ response);
//}

Console.ReadLine();





