
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
using System.Text;

int portForSub = 8987;
int portForPub = 8988;


IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
    x.AddDebug();
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Debug);
});
clientServices.AddNetMQMessageBroker("localhost", portForSub, "localhost", portForPub);
var services = clientServices.BuildServiceProvider();
using var broker = services.GetRequiredService<IMessageBrokerServer>();
broker.Start();

Console.ReadLine();
