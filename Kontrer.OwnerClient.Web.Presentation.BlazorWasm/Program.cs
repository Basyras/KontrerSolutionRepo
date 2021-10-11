using Kontrer.OwnerClient.Application.Customers;
using Kontrer.OwnerClient.Application.Orders;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.MessageBus;
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

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddMudServices();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMessageBus()
                .UseProxy()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            //builder.Services.AddSingleton<IOrderManager, OrderManager>();
            builder.Services.AddSingleton<IOrderManager, MockOrderManager>();
            //builder.Services.AddSingleton<ICustomerManager, CustomerManager>();
            builder.Services.AddSingleton<ICustomerManager, MockCustomerManager>();
            //var services = builder.Services.BuildServiceProvider();
            await builder.Build().RunAsync();
        }
    }
}