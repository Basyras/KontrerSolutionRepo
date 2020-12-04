using Kontrer.OwnerServer.Bootstrapper.Pricing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PricingServiceCollectionExtensions
    {
        public static PricingManagerBuilder AddPricing(this IServiceCollection services)
        {
            return new PricingManagerBuilder(services);
        }
    }
}
