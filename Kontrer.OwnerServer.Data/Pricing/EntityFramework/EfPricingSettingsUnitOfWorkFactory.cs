using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Pricing.EntityFramework
{
    public class EfPricingSettingsUnitOfWorkFactory : IUnitOfWorkFactory<IPricingSettingsUnitOfWork>
    {
        public IPricingSettingsUnitOfWork CreateUnitOfWork()
        {
            return new EfPricingSettingsUnitOfWork(new Data.EntityFramework.OwnerServerDbContext());
        }
    }
}
