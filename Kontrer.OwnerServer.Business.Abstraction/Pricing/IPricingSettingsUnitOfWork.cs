using Kontrer.OwnerServer.Business.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Pricing
{
    public interface IPricingSettingsUnitOfWork : IUnitOfWork
    {
        IPricingSettingsRepository PricingSettingsRepository { get; }
    }
}
