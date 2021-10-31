using Basyc.MessageBus.Manager;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.BusManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.AddBusManager()
                .AddReqeustClient<BasycMessageBusTypedRequestClient>()
                .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), new System.Reflection.Assembly[] { typeof(DeleteCustomerCommand).Assembly })
                .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddMessageBus()
                .UseProxy()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            var app = builder.Build();
            await app.RunAsync();
        }
    }
}