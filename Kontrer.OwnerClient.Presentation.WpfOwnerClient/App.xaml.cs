using Kontrer.OwnerClient.Bootstrapper;
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
        protected readonly Action<IServiceCollection> serviceConfiguration;

        public IHost AppHost { get; private set; }

        public App() : this(null)
        {

        }

        public App(Action<IServiceCollection> serviceConfiguration = null)
        {
            this.serviceConfiguration = serviceConfiguration;
            AppHost = CreateHost();
        }



        protected virtual void ConfigureServices(IServiceCollection service)
        {
            serviceConfiguration?.Invoke(service);
        }

        private IHost CreateHost()
        {

            return Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                ConfigureServices(services);
                var bootstrapper = new OwnerClientBootstrapper();
                foreach(var service in bootstrapper.BuildApp(services))
                {
                    services.Add(service);
                }

            }).Build();
        }


    }
}
