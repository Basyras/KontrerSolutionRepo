﻿using Kontrer.OwnerServer.PricingServer.Data.Pricing.EntityFramework;
using Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Data.Pricing.EntityFramework
{
    public class EfPricingSettingsUnitOfWorkFactory : IUnitOfWorkFactory<IPricingSettingsUnitOfWork>
    {
        public IPricingSettingsUnitOfWork CreateUnitOfWork()
        {
            return new EfPricingSettingsUnitOfWork(new PricingServiceDbContext());
        }
    }
}
