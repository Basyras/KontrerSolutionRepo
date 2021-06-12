using Kontrer.OwnerClient.Application.Pricing;
using MassTransit;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(x => new PricingSwaggerClient("https://localhost:44347/", x.GetRequiredService<HttpClient>()));

            //This code could be shared with namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((transitContext, rabbitConfig) =>
                {
#warning finish automatic consumer registration
                    //var settings = new EndpointSettings<ConsumerEndpointDefinition<IConsumer>>();
                    //new ConsumerEndpointDefinition<IConsumer>(settings)
                    //var definition = new NamedEndpointDefinition(context.HostingEnvironment.ApplicationName);
                    rabbitConfig.ConfigureEndpoints(transitContext);
                    //rabbitConfig.ReceiveEndpoint(context.HostingEnvironment.ApplicationName, c =>
                    //{                    
                    //});
                });

            });
            await builder.Build().RunAsync();
        }
    }
}
