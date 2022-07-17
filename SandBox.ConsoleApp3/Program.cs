
using Basyc.MessageBus.Broker;
using Basyc.MessageBus.Broker.NetMQ;
//using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
	x.AddDebug();
	x.AddConsole();
	x.SetMinimumLevel(LogLevel.Debug);
});
clientServices.AddNetMQMessageBroker();
var services = clientServices.BuildServiceProvider();
using var broker = services.GetRequiredService<IMessageBrokerServer>();


broker.Start();

Console.ReadLine();
