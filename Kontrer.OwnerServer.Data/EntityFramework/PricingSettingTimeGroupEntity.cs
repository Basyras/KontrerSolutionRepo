using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.EntityFramework
{
    public class PricingSettingTimeGroupEntity
    {

        public int PricingSettingTimeGroupEntityId { get; set; }
        public string TimeGroupName { get; set; }                
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<PricingTimedSettingEntity> TimedSettings { get; set; }



      
    }
}
