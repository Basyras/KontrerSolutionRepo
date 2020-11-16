using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kontrer.OwnerClient.Presentation.WpfOwnerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Action<IServiceCollection> serviceConfiguration;

        public IHost AppHost { get; private set; }

        public App() : this(null)
        {
           
        }

        public App(Action<IServiceCollection> serviceConfiguration = null)
        {
            this.serviceConfiguration = serviceConfiguration;
            ConfigureHost();
        }

     

        protected virtual void ConfifureServices(IServiceCollection service)
        {            
            serviceConfiguration?.Invoke(service);
        }

        private void ConfigureHost()
        {
            AppHost = Host.CreateDefaultBuilder().ConfigureServices(ConfifureServices).Build();
        }


    }
}
