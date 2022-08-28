// See https://aka.ms/new-console-template for more information
using Basyc.MessageBus.Client;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
	x.AddDebug();
	x.AddConsole();
	x.SetMinimumLevel(LogLevel.Debug);
});

clientServices
	.AddBasycMessageBusClient()
	.NoProxy()
	.RegisterBasycTypedHandlers<Program>()
	.SelectNetMQProvider("Console2");

var services = clientServices.BuildServiceProvider();

using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();
await client.StartAsync();


while (Console.ReadLine() != "stop")
{
	//client.SendAsync(new DeleteCustomerCommand(1)).GetAwaiter().GetResult();
	//client.PublishAsync(new CustomerCreatedEvent(new )).GetAwaiter().GetResult();
	Task.Run(() => client.SendAsync(new DeleteCustomerCommand(1)).GetAwaiter().GetResult());

}



Console.ReadLine();


