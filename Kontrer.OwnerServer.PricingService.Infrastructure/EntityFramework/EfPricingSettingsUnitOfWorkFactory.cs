using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class EfPricingSettingsUnitOfWorkFactory : IUnitOfWorkFactory<ISettingsUnitOfWork>
    {
        private readonly DbContextOptions<PricingServiceDbContext> contextOptions;

        public EfPricingSettingsUnitOfWorkFactory(DbContextOptions<PricingServiceDbContext> contextOptions)
        {
            this.contextOptions = contextOptions;
        }
        public ISettingsUnitOfWork CreateUnitOfWork()
        {
            
            return new EfPricingSettingsUnitOfWork(new PricingServiceDbContext(contextOptions));
        }
    }
}
