﻿// See https://aka.ms/new-console-template for more information
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
	.AddBasycBus()
	.NoProxy()
	.RegisterBasycTypedHandlers<Program>()
	.SelectNetMQProvider("Console1")
	.SelectHttpDiagnostics("https://localhost:7115/log");

var services = clientServices.BuildServiceProvider();
using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();

client.StartAsync();

while (Console.ReadLine() != "stop")
{
	var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "Console12", "aasdů"))
		.GetAwaiter()
		.GetResult();

	response.Switch(x => Console.WriteLine(x), x => Console.WriteLine(x));

	//client.PublishAsync<CustomerCreatedEvent>(new CustomerCreatedEvent(new CustomerEntity(1, "a", "aa", "aaa")))
	//	.GetAwaiter().GetResult();

}



Console.ReadLine();





