
using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.MessageBus.Broker;
//using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


IServiceCollection clientServices = new ServiceCollection();
clientServices
	.AddBasycDiagnosticsProducer()
	.SelectSignalR()
	.SetOptions(options =>
	{
		options.SignalRServerUri = "https://localhost:44310" + SignalRConstants.ProducersHubPattern;
	});

clientServices.AddLogging(x =>
{
	x.AddDebug();
	x.AddConsole();
	x.SetMinimumLevel(LogLevel.Debug);
});
clientServices.AddNetMQMessageBroker()
	.UseBasycDiagnosticsProducer();

var services = clientServices.BuildServiceProvider();
await services.StartBasycDiagnosticsProducer();
using var broker = services.GetRequiredService<IMessageBrokerServer>();
broker.Start();

Console.ReadLine();
