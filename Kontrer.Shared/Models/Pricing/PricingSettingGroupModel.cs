using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing
{
  

    public class PricingSettingGroupModel
    {
        public int SettingId { get; init; }        
        public string SettingName { get; init; }        
        public Type Type { get; init; }
        public List<TimedSettingModel> TimedSettings { get; init; } = new List<TimedSettingModel>();

        public PricingSettingGroupModel(Type type, int settingId, string settingName,List<TimedSettingModel> timedSettings)
        {
            Type = type;
            SettingId = settingId;
            TimedSettings = timedSettings;
            SettingName = settingName;
        }

        public PricingSettingGroupModel(Type type, int settingId, string settingName)
        {
            Type = type;
            SettingId = settingId;
            SettingName = settingName;
        }


    }
}
