using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingSettingTimeScopeEntity
    {

        public int PricingSettingTimeGroupEntityId { get; set; }
        public string TimeGroupName { get; set; }                
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<PricingScopedSettingEntity> ScopedSettings { get; set; }



      
    }
}
