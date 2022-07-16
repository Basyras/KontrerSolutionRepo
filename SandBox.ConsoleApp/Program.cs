// See https://aka.ms/new-console-template for more information
using Basyc.MessageBus.Client;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//var protobufSerializer = new ProtobufByteSerializer();

//var childResponse = new  ChildResponse("Jan2");
//var seriResultC = protobufSerializer.Serialize(childResponse, childResponse.GetType());
//var deseriResultC = protobufSerializer.Deserialize(seriResultC.AsT0, childResponse.GetType());

//var testResponse = new ParentResponse(new ChildResponse("Jan"));
//var seriResult = protobufSerializer.Serialize(testResponse, testResponse.GetType());
//var deseriResult = protobufSerializer.Deserialize(seriResult.AsT0, testResponse.GetType());

//var testResponse2 = new ParentResponse2(new ChildResponse("Jan2"));
//var seriResult2 = protobufSerializer.Serialize(testResponse2, testResponse2.GetType());
//var deseriResult2 = protobufSerializer.Deserialize(seriResult2.AsT0, testResponse2.GetType());


IServiceCollection clientServices = new ServiceCollection();
clientServices.AddLogging(x =>
{
	x.AddDebug();
	x.AddConsole();
	x.SetMinimumLevel(LogLevel.Debug);
});


clientServices
	.AddBasycMessageBusClient()
	.WithTypedMessages()
	.RegisterBasycTypedHandlers<Program>()
	.UseNetMQProvider("Console1");

var services = clientServices.BuildServiceProvider();
using ITypedMessageBusClient client = services.GetRequiredService<ITypedMessageBusClient>();

client.StartAsync();

while (Console.ReadLine() != "stop")
{
	var response = client.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new("Jan", "Console12", "aasdů"))
		.GetAwaiter()
		.GetResult();

	response.Switch(x => Console.WriteLine(x), x => Console.WriteLine(x));

}



Console.ReadLine();





