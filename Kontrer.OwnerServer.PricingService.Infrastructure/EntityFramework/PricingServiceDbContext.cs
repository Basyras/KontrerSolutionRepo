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
        public PricingServiceDbContext()
        {

        }

        //For unit testing
        public PricingServiceDbContext(DbContextOptions<PricingServiceDbContext> options) : base(options)
        {

        }

        public virtual DbSet<PricingSettingEntity> Settings { get; set; }
        public virtual DbSet<PricingSettingTimeScopeEntity> SettingTimeScopes { get; set; }
        public virtual DbSet<PricingScopedSettingEntity> ScopedSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricingScopedSettingEntity>().HasKey(x => new { x.SettingId, x.TimeScopeId });
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PricingSettingEntity>().Property(x => x.Type).HasConversion(x => x.AssemblyQualifiedName, x => Type.GetType(x));
        }

    }
}
