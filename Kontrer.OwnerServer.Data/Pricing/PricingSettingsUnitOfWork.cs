using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Pricing
{
    public class PricingSettingsUnitOfWork : IPricingSettingsUnitOfWork
    {
        public PricingSettingsUnitOfWork(IPricingSettingsRepository pricingSettingsRepository)
        {
            PricingSettingsRepository = pricingSettingsRepository;
        }

        public IPricingSettingsRepository PricingSettingsRepository { get; }

        public void Commit()
        {
            PricingSettingsRepository.Save();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return PricingSettingsRepository.SaveAsync(cancellationToken);
        }

        public void Dispose()
        {
            PricingSettingsRepository.Dispose();
        }
    }
}
