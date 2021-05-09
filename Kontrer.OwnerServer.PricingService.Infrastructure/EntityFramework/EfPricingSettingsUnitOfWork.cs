using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class EfPricingSettingsUnitOfWork : ISettingsUnitOfWork
    {
        private readonly PricingServiceDbContext dbContext;

        public EfPricingSettingsUnitOfWork(PricingServiceDbContext  dbContext)
        {
            PricingSettingsRepository = new EfPricingSettingsRepository(dbContext);
            this.dbContext = dbContext;
        }

        public ISettingsRepository PricingSettingsRepository { get; }

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
