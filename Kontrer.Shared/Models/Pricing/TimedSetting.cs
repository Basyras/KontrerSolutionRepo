using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing
{
    //public class TimedPricingSetting<TSettingValue>
    //{
    //    public int SettingId { get; init; }
    //    public string SettingName { get; set; }

    //    public TSettingValue Value { get; set; }

    //    public TimedPricingSetting(int settingId)
    //    {
    //        SettingId = settingId;
    //    }
    //}

    public class TimedSetting
    {
        public int SettingId { get; init; }
        public string SettingName { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        


        public TimedSetting(int settingId, Type type)
        {
            SettingId = settingId;
            Type = type;
        }
    }
}
