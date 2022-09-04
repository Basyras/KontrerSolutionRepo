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

var domains = new Assembly[]
{
	typeof(CustomerServiceDomainAssemblyMarker).Assembly,
	typeof(OrderServiceDomainAssemblyMarker).Assembly,
	typeof(IdGeneratorServiceDomainAssemblyMarker).Assembly,
};

builder.Services.AddBlazorMessageBus()
	.AddBusClient<TypedRequester>()
	.AddInterfacedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), domains)
	.SetDomainNameFormatter<TypedDDDDomainNameFormatter>();

builder.Services.AddBasycBusClient()
	.SelectHttpProxy()
	.SetProxyServerUri(new Uri("https://localhost:44310/"));



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.RunAsync();
