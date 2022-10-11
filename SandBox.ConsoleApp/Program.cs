﻿// See https://aka.ms/new-console-template for more information
using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.MessageBus.Client;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.AspNetCore.Builder;
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
	.AddBasycDiagnosticsProducing()
	.UseSignalR()
	.SetOptions(options =>
	{
		options.SignalRServerUri = "https://localhost:44310" + SignalRConstants.ProducersHubPattern;
	});


clientServices
	.AddBasycMessageBus()
	.NoProxy()
	.RegisterBasycTypedHandlers<Program>()
	.UseNetMQProvider("Console1")
	.UseDiagnostics("Console1")
	.UseBasycDiagnosticsLogExporter();
//.SelectHttpDiagnostics("https://localhost:7115/log");

var services = clientServices.BuildServiceProvider();
await services.StartBasycMessageBusClient();
using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();
await services.StartBasycDiagnosticsProducer();


while (Console.ReadLine() != "stop")
{
	var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "Console12", "aasdů"))
		.Task
		.GetAwaiter()
		.GetResult();

	response.Switch(x => Console.WriteLine(x), x => Console.WriteLine(x));

	//client.PublishAsync<CustomerCreatedEvent>(new CustomerCreatedEvent(new CustomerEntity(1, "a", "aa", "aaa")))
	//	.GetAwaiter().GetResult();

}



Console.ReadLine();





