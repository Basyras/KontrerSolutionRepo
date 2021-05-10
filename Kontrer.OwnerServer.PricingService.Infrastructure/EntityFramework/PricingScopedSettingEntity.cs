using System;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingScopedSettingEntity
    {
        [Key]
        public string PricingScopedSettingEntityId { get; set; }
        [Key]
        public DateTime From { get; set; }
        [Key]
        public DateTime To { get; set; }

        public PricingSettingEntity Setting { get; set; }
        public object Value { get; set; }
    



      

    }
}