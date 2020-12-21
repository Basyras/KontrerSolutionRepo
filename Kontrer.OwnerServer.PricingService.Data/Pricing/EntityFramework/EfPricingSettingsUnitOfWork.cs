using Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.PricingServer.Data.Pricing.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Data.Pricing.EntityFramework
{
    public class EfPricingSettingsUnitOfWork : IPricingSettingsUnitOfWork
    {
        private readonly PricingServiceDbContext dbContext;

        public EfPricingSettingsUnitOfWork(PricingServiceDbContext  dbContext)
        {
            PricingSettingsRepository = new EfPricingSettingsRepository(dbContext);
            this.dbContext = dbContext;
        }

        public IPricingSettingsRepository PricingSettingsRepository { get; }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
           return  dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
