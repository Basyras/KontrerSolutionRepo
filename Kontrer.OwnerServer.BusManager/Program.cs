using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.MessageBus;
using Kontrer.OwnerServer.BusManager;
using Kontrer.OwnerServer.CustomerService.Domain;
using Kontrer.OwnerServer.IdGeneratorService.Domain;
using Kontrer.OwnerServer.OrderService.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

//#if DEBUG
//await Task.Delay(5000);
//#endif


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var assembliesToScan = new Assembly[]
{
	typeof(CustomerServiceDomainAssemblyMarker).Assembly,
	typeof(OrderServiceDomainAssemblyMarker).Assembly,
	typeof(IdGeneratorServiceDomainAssemblyMarker).Assembly,
};

builder.Services.AddBasycDiagnosticExporting()
	.SetupDefaultService("BusManager")
	.AddSignalRExporter((options =>
	{
		options.SignalRServerUri = "https://localhost:44310" + SignalRConstants.ProducersHubPattern;
	}));

builder.Services.AddBasycMessageBus()
	.NoHandlers()
	.UseSignalRProxyProvider("https://localhost:44310")
	.UseDiagnostics()
		.ExportToBasycDiagnostics();

builder.Services.AddBasycDiagnosticReceiver()
	.SelectSignalR()
		.SetServerUri("https://localhost:44310");

var busManagerBuilder = builder.Services.AddBasycBusBlazorUI();
CreateTestingMessages(busManagerBuilder);

busManagerBuilder.RegisterMessagesFromAssembly(assembliesToScan)
	.RegisterMessagesAsCQRS(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>))
	.UseBasycDiagnosticsReceivers()
		.UseMapper<BusManagerBasycDiagnosticsReceiverTraceIDMapper>()
	.UseBasycTypedMessageBusRequester()
		.SetDomainNameFormatter<TypedDddDomainNameFormatter>();



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var app = builder.Build();
await app.Services.StartBasycDiagnosticsReceivers();
await app.Services.StartBasycDiagnosticExporters();
await app.Services.StartBasycMessageBusClient();
await app.RunAsync();

static void CreateTestingMessages(Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.BusManagerApplicationBuilder busManagerBuilder)
{
	busManagerBuilder.RegisterMessagesViaFluentApi()
					.AddDomain("Domain_ManualParams")
						//NoReturn
						.AddMessage("Params:Manual_Return:No_HandeledBy:RequestResult")
							.WithParameter<string>("Name")
							.NoReturn()
							.HandeledBy((RequestContext requestResult) =>
							{
								//requestResult.Start();
								var rootSegment = requestResult.StartNewSegment("R dur 450");
								var nestedA = rootSegment.StartNested("R.A: +-0 before duration 50");
								Thread.Sleep(50);
								var nestedB = nestedA.EndAndStartFollowing("R.B: 0 before, duration 200");
								var nestedBA = nestedB.StartNested("R.B.A: +-0 before duration 150");
								Thread.Sleep(150);
								nestedBA.End();
								Thread.Sleep(50);
								var nestedBB = nestedB.StartNested("R.B.B  50 before, duration +-0");
								nestedBB.End();
								var nestedBC = nestedB.StartNested("R.B.C  +-0 before, duration +-0");
								nestedBC.End();
								Thread.Sleep(100);
								var nestedBD = nestedB.StartNested("R.B.D  100 before, duration 100");
								Thread.Sleep(100);

								nestedBD.End();
								nestedB.End();
								rootSegment.End();
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
							.HandeledBy((RequestContext requestResult) =>
							{
								var rootSegment = requestResult.StartNewSegment("R duration 200");
								var ra = rootSegment.StartNested("R.A 50");
								Thread.Sleep(50);
								var rb = ra.EndAndStartFollowing("R.B 50");
								Thread.Sleep(50);
								var rc = rb.EndAndStartFollowing("R.C 50");
								Thread.Sleep(50);
								var rd = rc.EndAndStartFollowing("R.D 50");
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
							.HandeledBy((RequestContext requestResult) => { requestResult.Complete(420); })
						.AddMessage("Params:Manual_Return:T_HandeledBy:RequestTReturn")
							.WithParameter<string>("Name")
							.Returns<string>("ResponseTypeDisplayName")
							.HandeledBy((Request request) => "responseString")
					.AddDomain("Domain_ClassParams")
						//NoReturn
						.AddMessage("Params:TMessage_Return:No_HandeledBy:RequestResult")
							.WithParameters<DummyMessage>()
							.NoReturn()
							.HandeledBy((RequestContext requestResult) => requestResult.Complete())
						.AddMessage("Params:TMessage_Return:No_HandeledBy:Request")
							.WithParameters<DummyMessage>()
							.NoReturn()
							.HandeledBy((Request request) => { })
						//ReturnTypeOf
						.AddMessage("Params:TMessage_Return:TypeOf_HandeledBy:RequestResult")
							.WithParameters<DummyMessage>()
							.Returns(typeof(int), "ResponseTypeDisplayName")
							.HandeledBy((RequestContext requestResult) => requestResult.Complete(5))
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
							.HandeledBy((RequestContext requestResult) => requestResult.Complete("asdas"))
						.AddMessage("Params:TMessage_Return:T_HandeledBy:RequestResult")
							.WithParameters<DummyMessage>()
							.Returns<int>("CreateCustomerMessageResponse")
							.HandeledBy((RequestContext requestResult) => requestResult.Complete(5))
						.AddMessage("Params:TMessage_Return:T_HandeledBy:TMessageTReturn")
							.WithParameters<DummyMessage>()
							.Returns<int>("int number")
							.HandeledBy((Request request) => 7)
						.AddMessage("Params:TMessage_Return:T_HandeledBy:TMessageTReturn")
							.WithParameters<DummyMessage>()
							.Returns<string>("text")
							.HandeledBy((DummyMessage message) => "asd");
}

public record DummyMessage(string Name, int Age);