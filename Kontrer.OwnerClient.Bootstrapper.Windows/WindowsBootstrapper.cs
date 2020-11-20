using Kontrer.OwnerClient.UI.Abstraction;
using Kontrer.OwnerClient.UI.WPF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Bootstrapper.Windows
{
    public class WindowsBootstrapper
    {
        public WindowsBootstrapper(string[] args)
        {
            Args = args;
        }

        public string[] Args { get; }

        public IServiceCollection Create(IServiceCollection services)
        {
            services.AddSingleton<IOwnerClientUI, WPFOwnerClientUI>();
            return services;
        }
    }
}
