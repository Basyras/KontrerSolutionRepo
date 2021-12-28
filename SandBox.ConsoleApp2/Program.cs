// See https://aka.ms/new-console-template for more information
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

IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
    x.AddDebug();
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Debug);
});

clientServices
    .AddMessageBusClient()
    .AddNetMQClient(portForPub, portForSub, "Console2");

var services = clientServices.BuildServiceProvider();

using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();
client.StartAsync();



while (Console.ReadLine() != "stop")
{
    client.SendAsync(new DeleteCustomerCommand(1)).GetAwaiter().GetResult();
}



Console.ReadLine();


