using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Application;
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

var busUIBuilder = builder.Services.AddBasycBusBlazorUI();
busUIBuilder.RegisterMessagesViaFluentApi()
				.AddDomain("Domain_ManualParams")
					//NoReturn
					.AddMessage("Params:Manual_Return:No_HandeledBy:RequestResult")
						.WithParameter<string>("Name")
						.NoReturn()
						.HandeledBy((RequestResult requestResult) =>
						{
							//requestResult.Start();
							var rootSegment = requestResult.StartNewSegment("R dur 450");
							var nestedA = rootSegment.StartNewNestedSegment("R.A: +-0 before duration 50");
							Thread.Sleep(50);
							var nestedB = nestedA.EndAndStartNewFollowingSegment("R.B: 0 before, duration 200");
							var nestedBA = nestedB.StartNewNestedSegment("R.B.A: +-0 before duration 150");
							Thread.Sleep(150);
							nestedBA.End();
							Thread.Sleep(50);
							var nestedBB = nestedB.StartNewNestedSegment("R.B.B  50 before, duration +-0");
							nestedBB.End();
							var nestedBC = nestedB.StartNewNestedSegment("R.B.C  +-0 before, duration +-0");
							nestedBC.End();
							Thread.Sleep(100);
							var nestedBD = nestedB.StartNewNestedSegment("R.B.D  100 before, duration 100");
							Thread.Sleep(100);
							requestResult.Complete();
						})
					.AddMessage("Params:Manual_Return:No_HandeledBy:Request")
						.WithParameter<string>("Name")
						.NoReturn()
						.HandeledBy((Request request) => { })
					//ReturnTypeOf
					.AddMessage("<TEST> Params:Manual_Return:TypeOf_HandeledBy:RequestResult")
						.WithParameter<string>("Name")
						.Returns(typeof(string), "ResponseTypeDisplayName")
						.HandeledBy((RequestResult requestResult) =>
						{
							var rootSegment = requestResult.StartNewSegment("R duration 200");
							var ra = rootSegment.StartNewNestedSegment("R.A 50");
							Thread.Sleep(50);
							var rb = ra.EndAndStartNewFollowingSegment("R.B 50");
							Thread.Sleep(50);
							var rc = rb.EndAndStartNewFollowingSegment("R.C 50");
							Thread.Sleep(50);
							var rd = rc.EndAndStartNewFollowingSegment("R.D 50");
							Thread.Sleep(50);
							requestResult.Complete("returnString");
						})
					.AddMessage("ParamsManual_ReturnTypeof_HandeledByRequest")
						.WithParameter<string>("Name")
						.Returns(typeof(string), "text")
						.HandeledBy((Request request) => "responseString")
					//ReturnT
					.AddMessage("Params:Manual_Return:T_HandeledBy:RequestResult")
						.WithParameter<string>("Name")
						.Returns<int>("int number")
						.HandeledBy((RequestResult requestResult) => { requestResult.Complete(420); })
					.AddMessage("Params:Manual_Return:T_HandeledBy:RequestTReturn")
						.WithParameter<string>("Name")
						.Returns<string>("ResponseTypeDisplayName")
						.HandeledBy((Request request) => "responseString")
				.AddDomain("Domain_ClassParams")
					//NoReturn
					.AddMessage("Params:TMessage_Return:No_HandeledBy:RequestResult")
						.WithParameters<DummyMessage>()
						.NoReturn()
						.HandeledBy((RequestResult requestResult) => requestResult.Complete())
					.AddMessage("Params:TMessage_Return:No_HandeledBy:Request")
						.WithParameters<DummyMessage>()
						.NoReturn()
						.HandeledBy((Request request) => { })
					//ReturnTypeOf
					.AddMessage("Params:TMessage_Return:TypeOf_HandeledBy:RequestResult")
						.WithParameters<DummyMessage>()
						.Returns(typeof(int), "ResponseTypeDisplayName")
						.HandeledBy((RequestResult requestResult) => requestResult.Complete(5))
					.AddMessage("Params:TMessage_ReturnTypeOf_HandeledByRequest")
						.WithParameters<DummyMessage>()
						.Returns(typeof(int), "ResponseTypeDisplayName")
						.HandeledBy((Request request) => 6)
					.AddMessage("Params:TMessage_ReturnTypeOf_HandeledByTMessageObjectReturn")
						.WithParameters<DummyMessage>()
						.Returns(typeof(int), "int number")
						.HandeledBy((DummyMessage message) => (object)message.Age)
					.AddMessage("Params:TMessage_ReturnTypeOf_HandeledByTMessageTReturn")
						.WithParameters<DummyMessage>()
						.Returns(typeof(int), "int number")
						.HandeledBy<int>((message) => message.Age)
					//ReturnT
					.AddMessage("Params:TMessage_Return:T_HandeledBy:RequestResult")
						.WithParameters<DummyMessage>()
						.Returns<string>("CreateCustomerMessageResponse")
						.HandeledBy((RequestResult requestResult) => requestResult.Complete("asdas"))
					.AddMessage("Params:TMessage_Return:T_HandeledBy:RequestResult")
						.WithParameters<DummyMessage>()
						.Returns<int>("CreateCustomerMessageResponse")
						.HandeledBy((RequestResult requestResult) => requestResult.Complete(5))
					.AddMessage("Params:TMessage_Return:T_HandeledBy:TMessageTReturn")
						.WithParameters<DummyMessage>()
						.Returns<int>("int number")
						.HandeledBy((Request request) => 7)
					.AddMessage("Params:TMessage_Return:T_HandeledBy:TMessageTReturn")
						.WithParameters<DummyMessage>()
						.Returns<string>("text")
						.HandeledBy((DummyMessage message) => "asd");


busUIBuilder.RegisterMessagesFromAssembly(assembliesToScan)
	.RegisterMessagesAsCQRS(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>))
	.SelectTypedRequester()
	.SetDomainNameFormatter<TypedDddDomainNameFormatter>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.RunAsync();

public record DummyMessage(string Name, int Age);