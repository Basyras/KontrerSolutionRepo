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

            builder.Services.AddSingleton<IOrderManager, OrderManager>();
            builder.Services.AddSingleton<ICustomerManager, CustomerManager>();
            var services = builder.Services.BuildServiceProvider();

            await SeedData(services);
            await builder.Build().RunAsync();
        }

        private static async Task SeedData(IServiceProvider services)
        {
            var customerMana = services.GetRequiredService<ICustomerManager>();
            var customers = await customerMana.GetCustomers();
            foreach (var customer in customers)
            {
                await customerMana.DeleteCustomer(customer.Id);
            }
            var cus1 = await customerMana.CreateCustomer("Jan", "Hadašèok", "asdasd@asdasd.cz");
            var cus2 = await customerMana.CreateCustomer("Petr", "Novotny", "asdasd@asdasd.cz");
            var cus3 = await customerMana.CreateCustomer("Jaroslava", "Bezecna", "asdasd@asdasd.cz");

            var orderMana = services.GetRequiredService<IOrderManager>();
            var orders = await orderMana.GetOrders();
            foreach (var order in orders)
            {
                await orderMana.DeleteOrder(order.Order.Id);
            }
            var newOrder1 = await orderMana.CreateOrder(cus1.Id);
            var newOrder2 = await orderMana.CreateOrder(cus2.Id);
            var newOrder3 = await orderMana.CreateOrder(cus3.Id);
        }
    }
}