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

int portForSub = 8987;
int portForPub = 8988;

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

using IMessageBusClient client = services.GetRequiredService<IMessageBusClient>();
(client as NetMQMessageBusClient)!.StartAsync();

void Test()
{
    while (Console.ReadLine() != "stop")
    {
        var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan","Console12","aasdů"))
            .GetAwaiter()
            .GetResult();
    }
}

Test();


