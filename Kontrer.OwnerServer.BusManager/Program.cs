using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus.Manager;
using Kontrer.OwnerServer.CustomerService.Domain;
using Kontrer.OwnerServer.IdGeneratorService.Domain;
using Kontrer.OwnerServer.OrderService.Domain;
using Kontrer.OwnerServer.BusManager;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using Basyc.MessageBus.Client;

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
    .AddBusClient<BasycInterfaceTypedBusClient>()
    .AddInterfacedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), domains)
    .SetDomainNameFormatter<TypedDDDDomainNameFormatter>();

builder.Services.AddMessageBusClient()
    .AddProxyProvider()
    .SetProxyServerUri(new Uri("https://localhost:44310/"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
