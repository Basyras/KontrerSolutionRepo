using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Business.Pricing;
using Kontrer.OwnerServer.Presentation.AspApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

  
namespace Kontrer.OwnerServer.Bootstrapper
{
    public class AspApiBootstrapper
    {
        public AspApiBootstrapper()
        {

        }

        public IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureServices(ConfigureServers);
            });
        }

        private void ConfigureServers(IServiceCollection services)
        {
            //services.AddSingleton<IPricingManager, PricingManager>();
            services.AddPricing();
        }

    }



}
