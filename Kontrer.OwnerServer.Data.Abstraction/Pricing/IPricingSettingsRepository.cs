using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;

namespace Kontrer.OwnerServer.Data.Abstraction.Pricing
{
    public interface IPricingSettingsRepository : IGenericRepository<PricingSettingsModel,int>
    {
      
    }
}
