using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingSettingGroupEntity
    {
        [Key]
        public int PricingSettingGroupId { get; set; }
        public string SettingName { get; set; }
        public Type Type { get; set; }
        public List<PricingScopedSettingEntity> TimedSettings { get; set; } = new List<PricingScopedSettingEntity>();
     
    }
}