using System;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.PricingServer.Data.Pricing.EntityFramework
{
    public class PricingTimedSettingEntity
    {
        public PricingSettingGroupEntity PricingSettingGroup { get; set; }
        [Key]
        public int PricingSettingGroupId { get; set; }
        public object Value { get; set; }
        [Key]
        public DateTime Start { get; set; }
        [Key]
        public DateTime End { get; set; }



      

    }
}