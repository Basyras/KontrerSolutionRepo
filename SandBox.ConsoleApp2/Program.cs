﻿// See https://aka.ms/new-console-template for more information
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
using System.Text;


////Producer
int portForSub = 8987;
int portForPub = 8988;
int portForPull = 4558;
int portForPush = 5557;
int brokerPort = 5357;

IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
    x.AddDebug();
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Debug);
});
clientServices
    .AddMessageBusClient()
    .AddNetMQProvider(portForPub, portForSub, portForPush, portForPull, 5357, "client2");

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

//using (var server = new ResponseSocket())
//{
//    server.Bind("tcp://*:5555");
//    while (true)
//    {
//        var message = server.ReceiveFrameString();
//        Console.WriteLine("Received {0}", message);
//        // processing the request
//        Thread.Sleep(100);
//        Console.WriteLine("Sending World");
//        server.SendFrame("World");
//    }
//}


//using var server = new RouterSocket("@tcp://127.0.0.1:5555");
//while (true)
//{
//    var clientMessage = server.ReceiveMultipartMessage();
//    if (clientMessage.FrameCount == 3)
//    {
//        var clientAddress = clientMessage[0];
//        var clientOriginalMessage = clientMessage[2].ConvertToString();
//        string response = string.Format("{0} back from server {1}",
//            clientOriginalMessage, DateTime.Now.ToLongTimeString());
//        var messageToClient = new NetMQMessage();
//        messageToClient.Append(clientAddress);
//        messageToClient.AppendEmptyFrame();
//        messageToClient.Append(response);
//        server.SendMultipartMessage(messageToClient);
//    }
//}

//using var poller = new NetMQPoller();
//DealerSocket client = new DealerSocket();
//client = new DealerSocket();
//client.Options.Identity = Encoding.Unicode.GetBytes("Client-ConsoleApp2");
//client.Connect($"tcp://localhost:{brokerPort}");
//client.ReceiveReady += Client_ReceiveReady;
//poller.Add(client);
//poller.RunAsync();
//var messageToServer = new NetMQMessage();
//messageToServer.AppendEmptyFrame();
//messageToServer.Append("Client-ConsoleApp2 request1");
//Console.WriteLine("Sending to server");
//client.SendMultipartMessage(messageToServer);
//Console.ReadLine();

//void Client_ReceiveReady(object? sender, NetMQSocketEventArgs? e)
//{
//    var response = e.Socket.ReceiveFrameString();
//    Console.WriteLine("Message from server recieved, " + response);
//}

Console.ReadLine();


