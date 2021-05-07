
using Kontrer.OwnerServer.PricingService.Infrastructure.Abstraction.Pricing;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.Abstraction.Pricing
{
    public interface IPricingSettingsUnitOfWork : IUnitOfWork
    {
        IPricingSettingsRepository PricingSettingsRepository { get; }
    }
}