using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication1;

namespace Basyc.MessageBus.Manager.Presentation.Blazor
{
    public class MessageBusManagerBlazorAppBuilder
    {
        private static WebAssemblyHostBuilder blazorBuilder;

        public static BusManagerBuilder Create(string[] args)
        {
            blazorBuilder = WebAssemblyHostBuilder.CreateDefault(args);
            blazorBuilder.RootComponents.Add<App>("#app");
            blazorBuilder.Services.AddMudServices();
            blazorBuilder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(blazorBuilder.HostEnvironment.BaseAddress) });
            //blazorBuilder.Services.AddSingleton(blazorBuilder);

            var managerBuilder = blazorBuilder.Services.AddMessageManager();
            return managerBuilder;
        }

        public static MessageBusManagerApp Build()
        {
            return new MessageBusManagerApp(blazorBuilder.Build());
        }
    }
}