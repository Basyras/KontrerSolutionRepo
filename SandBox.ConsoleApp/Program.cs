using Basyc.MessageBus.Client;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
	//x.AddDebug();
	x.AddConsole();
	x.SetMinimumLevel(LogLevel.Debug);
});

clientServices.AddBasycDiagnosticExporting()
	.SetDefaultIdentity("Console1 - Consumer")
	.AddSignalRExporter("https://localhost:44310")
.AutomaticallyExport()
	.AnyActvity()
	.AnyLog();


clientServices.AddBasycMessageBus()
		.RegisterBasycTypedHandlers<Program>()
		.SelectNetMQProvider("Console1")
		.UseDiagnostics();

var services = clientServices.BuildServiceProvider();
await services.StartBasycMessageBusClient();
await services.StartBasycDiagnosticExporters();


using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();
while (Console.ReadLine() != "stop")
{
	var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "Console12", "aasdů"))
		.Task
		.GetAwaiter()
		.GetResult();

	response.Switch(x => Console.WriteLine(x), x => Console.WriteLine(x));
}



Console.ReadLine();





