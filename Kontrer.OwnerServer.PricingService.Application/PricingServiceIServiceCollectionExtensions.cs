using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application
{
    public static class PricingServiceIServiceCollectionExtensions
    {
        public static PricingServiceBuilder AddPricingService(this IServiceCollection services)
        {
            services.AddSingleton<PricingManager>();
            return new PricingServiceBuilder(services);            
        }
    }
}
