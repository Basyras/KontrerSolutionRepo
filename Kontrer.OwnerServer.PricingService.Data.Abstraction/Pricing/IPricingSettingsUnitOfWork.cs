
using Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing
{
    public interface IPricingSettingsUnitOfWork : IUnitOfWork
    {
        IPricingSettingsRepository PricingSettingsRepository { get; }
    }
}