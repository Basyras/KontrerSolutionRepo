using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Business.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Pricing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Data.Pricing.EntityFramework;
using Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares;
using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;

namespace Kontrer.OwnerServer.Bootstrapper.Pricing
{
    public class PricingManagerBuilder
    {
        private readonly IServiceCollection services;
        public PricingManagerBuilder(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IPricingManager, PricingManager>();

            AddEfStores();
            AddAccommodationPricing();

            //services.Scan(scan => 
            //scan.FromAssemblyOf<PricingManager>()
            //.AddClasses(x=>x.InNamespaceOf<PricingManager>())
            //.AsImplementedInterfaces()

            //.FromAssemblyOf<PricingSettingsRepository>()
            //.AddClasses(x => x.InNamespaceOf<PricingSettingsRepository>())
            //.AsImplementedInterfaces());
        }

        private PricingManagerBuilder AddAccommodationPricing()
        {
            services.AddSingleton<IAccommodationBlueprintEditor, AddCustomerDiscountEditor>();
            services.AddSingleton<IAccommodationPricingMiddleware, BaseCostMiddleware>();
            services.AddSingleton<IAccommodationPricingMiddleware, ItemTaxMiddleware>();

          

            return this;
        }

        private PricingManagerBuilder AddEfStores()
        {
            services.AddSingleton<IUnitOfWorkFactory<IPricingSettingsUnitOfWork>, EfPricingSettingsUnitOfWorkFactory>();
            return this;
        }






    }
}
