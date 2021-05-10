using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingSettingEntity
    {
        [Key]
        public string PricingSettingEntityId { get; set; }        
        public Type Type { get; set; }
        public List<PricingScopedSettingEntity> ScopedSettings { get; set; }
     
    }
}