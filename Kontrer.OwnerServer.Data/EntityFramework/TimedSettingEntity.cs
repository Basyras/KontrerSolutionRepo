using System;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.Data.EntityFramework
{
    public class TimedSettingEntity
    {
        public PricingSettingGroupEntity PricingSettingGroup { get; set; }
        [Key]
        public int PricingSettingGroupId { get; set; }
#warning Should be json string / or grpc bytes
        public object Value { get; set; }
        [Key]
        public DateTime Start { get; set; }
        [Key]
        public DateTime End { get; set; }



      

    }
}