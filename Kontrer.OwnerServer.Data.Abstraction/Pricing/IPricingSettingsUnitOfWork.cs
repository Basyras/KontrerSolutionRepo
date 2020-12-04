
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Pricing
{
    public interface IPricingSettingsUnitOfWork : IUnitOfWork
    {
        IPricingSettingsRepository PricingSettingsRepository { get; }
    }
}