using Kontrer.OwnerServer.PricingService.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public static class PricingServiceInfrastructureIServiceCollectionExtensions
    {
        public static PricingServiceBuilder AddEFRepository(this PricingServiceBuilder builder)
        {
            builder.AddRepository<EfPricingSettingsRepository>();
            return builder;
        }
    }
}
