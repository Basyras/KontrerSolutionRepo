using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class EfPricingSettingsUnitOfWorkFactory : IUnitOfWorkFactory<ISettingsUnitOfWork>
    {
        public ISettingsUnitOfWork CreateUnitOfWork()
        {
            return new EfPricingSettingsUnitOfWork(new PricingServiceDbContext());
        }
    }
}
