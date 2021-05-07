using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingServer.Data.Pricing.EntityFramework
{
    public class PricingServiceDbContext : DbContext
    {      
        public virtual DbSet<PricingSettingGroupEntity> PricingSettingGroups { get; set; }
        public virtual DbSet<PricingTimedSettingEntity> PricingTimedSettings { get; set; }
        public virtual DbSet<PricingSettingTimeGroupEntity> PricingSettingTimeGroups { get; set; }
     
    }
}
