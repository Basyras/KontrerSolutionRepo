using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus.Manager;
using Kontrer.OwnerServer.BusManager;
using Kontrer.OwnerServer.CustomerService.Domain;
using Kontrer.OwnerServer.IdGeneratorService.Domain;
using Kontrer.OwnerServer.OrderService.Domain;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

#if DEBUG
await Task.Delay(5000);
#endif


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var assembliesToScan = new Assembly[]
{
	typeof(CustomerServiceDomainAssemblyMarker).Assembly,
	typeof(OrderServiceDomainAssemblyMarker).Assembly,
	typeof(IdGeneratorServiceDomainAssemblyMarker).Assembly,
};

builder.Services.AddBasycBus()
	.SelectHttpProxy()
	.SetProxyServerUri(new Uri("https://localhost:44310/"));

//builder.Services.AddBasycBusBlazorUI()
//	.SelectRequester<TypedRequester>()
//	.RegisterCQRSMessages(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), domains)
//	.SetDomainNameFormatter<TypedDDDDomainNameFormatter>();

var busUIBuilder = builder.Services.AddBasycBusBlazorUI();

busUIBuilder.RegisterMessagesViaFluentApi()
				.AddDomain("CustomerDomain")
					.AddMessage("CreateCustomerMessage")
						.WithParameter<int>("CustomerID")
						.WithParameter<string>("FirstName")
						.Returns<string>("CreateCustomerMessageResponse")
						.HandeledBy(requestResult => requestResult.Complete(TimeSpan.FromSeconds(5), "Customer created"))
					.AddMessage("DummyClassMessage")
						.WithParameters<DummyClass>()
						.Returns<string>("CreateCustomerMessageResponse")
						.HandeledBy((message, requestResult) => requestResult.Complete(TimeSpan.FromSeconds(5), message.Age.ToString()))
				.AddDomain("OrderDomain")
					.AddMessage("CreateOrderMessage")
						.WithParameter<int>("OrderID")
						.WithParameter<string>("OrderName")
						.HandeledBy(requestResult => requestResult.Complete(TimeSpan.FromSeconds(5), "Order created"));

busUIBuilder.RegisterMessagesFromAssembly(assembliesToScan)
	.RegisterMessagesAsCQRS(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>))
	.SelectTypedRequester()
	.SetDomainNameFormatter<TypedDddDomainNameFormatter>();



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.RunAsync();

public record DummyClass(string Name, int Age);