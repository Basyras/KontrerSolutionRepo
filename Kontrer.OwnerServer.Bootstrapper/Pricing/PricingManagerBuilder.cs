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

namespace Kontrer.OwnerServer.Bootstrapper.Pricing
{
    public class PricingManagerBuilder
    {
        private readonly IServiceCollection services;
        public PricingManagerBuilder(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IPricingManager, PricingManager>();            
            services.AddSingleton<IUnitOfWorkFactory<PricingSettingsUnitOfWork>, IUnitOfWorkFactory<PricingSettingsUnitOfWork>>();

            //services.Scan(scan => 
            //scan.FromAssemblyOf<PricingManager>()
            //.AddClasses(x=>x.InNamespaceOf<PricingManager>())
            //.AsImplementedInterfaces()

            //.FromAssemblyOf<PricingSettingsRepository>()
            //.AddClasses(x => x.InNamespaceOf<PricingSettingsRepository>())
            //.AsImplementedInterfaces());            

            

        }

      

     


    }
}
