using Basyc.MessageBus.Broker;
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

clientServices
	.AddBasycDiagnosticExporting()
	.SetDefaultService("Console3 - Broker")
	.AddSignalRExporter("https://localhost:44310");


clientServices.AddBasycNetMQMessageBroker()
	.UseBasycDiagnosticsProducer();

var services = clientServices.BuildServiceProvider();
await services.StartBasycDiagnosticExporters();
using var broker = services.GetRequiredService<IMessageBrokerServer>();
broker.Start();

Console.ReadLine();
