
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
clientServices.AddNetMQMessageBroker("localhost", portForSub, "localhost", portForPub,"localhost", brokerPort);
var services = clientServices.BuildServiceProvider();
using var broker = services.GetRequiredService<IMessageBrokerServer>();
broker.Start();


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


//using var server = new RouterSocket($"@tcp://localhost:{brokerPort}");
//while (true)
//{
//    var clientMessage = server.ReceiveMultipartMessage();

//    if (clientMessage.FrameCount == 3)
//    {
//        var clientAddress = clientMessage[0];

//        Console.WriteLine($"Request recieved from client: {clientAddress.ConvertToString()}");
//        string clientOriginalMessage = clientMessage[2].ConvertToString();
//        string response = "response";
//        var messageToClient = new NetMQMessage();
//        messageToClient.Append(clientAddress);
//        messageToClient.AppendEmptyFrame();
//        messageToClient.Append(response);
//        server.SendMultipartMessage(messageToClient);
//    }
//    else
//    {
//        Console.WriteLine($"Request recieved with unexpected format");
//    }
//}

Console.ReadLine();
