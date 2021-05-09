using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingServiceDbContext : DbContext
    {      
        public virtual DbSet<PricingSettingGroupEntity> PricingSettingGroups { get; set; }
        public virtual DbSet<PricingScopedSettingEntity> PricingScopedSettings { get; set; }
        public virtual DbSet<PricingSettingTimeScopeEntity> PricingSettingTimeScopes { get; set; }
     
    }
}
