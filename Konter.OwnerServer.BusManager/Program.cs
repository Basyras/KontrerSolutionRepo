using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Presentation.Blazor;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Konter.OwnerServer.BusManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddBlazorMessageBus()
            //    .AddBusClient<BasycInterfaceTypedBusClient>()
            //    .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), typeof(CreateCustomerCommand).Assembly)
            //    .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddBlazorMessageBus()
                .AddBusClient<BasycInterfaceTypedBusClient>()
                .AddInterfaceTypedProvider(typeof(IRequest), typeof(IRequest<>), typeof(CreateCustomerCommand).Assembly)
                .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddMessageBus()
                .AddProxyProvider()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}