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
        public virtual DbSet<PricingSettingEntity> Settings { get; set; }
        public virtual DbSet<PricingSettingScopeEntity> SettingScopes { get; set; }
        public virtual DbSet<PricingScopedSettingEntity> ScopedSettings { get; set; }
     
    }
}
