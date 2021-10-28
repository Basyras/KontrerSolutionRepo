using Basyc.MessageBus.Manager.Application;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.Blazor
{
    public class MessageBusManagerApp
    {
        private readonly WebAssemblyHost blazorHost;

        public MessageBusManagerApp(WebAssemblyHost blazorHost)
        {
            this.blazorHost = blazorHost;
        }

        public async Task RunAsync()
        {
            var explorer = blazorHost.Services.GetRequiredService<IMessageManager>();
            explorer.Initialize();
            await blazorHost.RunAsync();
        }
    }
}