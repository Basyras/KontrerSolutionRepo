using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Presentation.Blazor;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.Shared.DomainDrivenDesign.Domain;
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
        public static async Task MainOri(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var assemblies = new Assembly[] { typeof(DeleteCustomerCommand).Assembly };
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMessageManager()
                .AddReqeustClient<BasycMessageBusTypedRequestClient>()
                .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assemblies)
                .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddMessageBus()
                .AddProxyProvider()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            await builder.Build().RunAsync();
        }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            //builder.Services.AddMudServices();

            var assemblies = new Assembly[] { typeof(CreateCustomerCommand).Assembly };

            //builder.Services.AddMessageManager()
            //    .AddReqeustClient<BasycMessageBusTypedRequestClient>()
            //    .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assemblies)
            //    .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddBlazorMessageBus()
                .AddReqeustClient<BasycMessageBusTypedRequestClient>()
                .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assemblies)
                .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddMessageBus()
                .AddProxyProvider()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44371/") });

            var host = builder.Build();
            //var tt = host.Services.GetRequiredService<BusManagerJSInterop>();
            //await tt.ApplyChangesToIndexHtml();
            //var askmsg = Task.Run(async () => await tt.ApplyChangesToIndexHtml()).;
            await host.RunAsync();
        }
    }
}