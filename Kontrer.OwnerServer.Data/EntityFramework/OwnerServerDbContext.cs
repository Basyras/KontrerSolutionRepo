using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using Kontrer.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.EntityFramework
{
    public class OwnerServerDbContext : DbContext
    {      
        public virtual DbSet<PricingSettingGroupEntity> PricingSettingGroups { get; set; }
        public virtual DbSet<PricingTimedSettingEntity> PricingTimedSettings { get; set; }
        public virtual DbSet<PricingSettingTimeGroupEntity> PricingSettingTimeGroups { get; set; }
     
    }
}
