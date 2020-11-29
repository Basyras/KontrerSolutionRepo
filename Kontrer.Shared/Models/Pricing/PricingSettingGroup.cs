using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing
{
  

    public class PricingSettingGroup
    {
        public int SettingId { get; init; }
        public string SettingName { get; set; }
        public Type Type { get; init; }
        public List<TimedSetting> TimedSettings { get; set; } = new List<TimedSetting>();

        public PricingSettingGroup(Type type)
        {
            Type = type;
        }

     
    }
}
