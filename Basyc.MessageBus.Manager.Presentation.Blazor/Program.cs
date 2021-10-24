using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Basyc;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.IdGeneratorService.Domain;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
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

namespace Basyc.MessageBus.Manager.Presentation.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddMudServices();

            var assemblies = new Assembly[] { typeof(CreateNewIdCommand).Assembly, typeof(DeleteAccommodationOrderCommand).Assembly, typeof(CreateCustomerCommand).Assembly };

            builder.Services.AddMessageExplorer()
                .UseReqeustClient<BasycMessageBusTypedRequestClient>()
                .UseTypedCQRSProvider(typeof(IRequest<>), typeof(IRequest), typeof(IRequest<>), assemblies)
                //.UseTypedGenericProvider(typeof(IRequest), typeof(IRequest<>), assemblies)
                .UseDomainNameFormatter<TypedDDDDomainNameFormatter>();

            builder.Services.AddMessageBus()
                .UseProxy()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();
            var explorer = host.Services.GetRequiredService<IMessageManager>();
            explorer.Initialize();
            await host.RunAsync();
        }
    }
}