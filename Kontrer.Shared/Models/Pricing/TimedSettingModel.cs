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

    public class TimedSettingModel
    {
        public int SettingGroupId { get; init; }
        public string SettingName { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }



        public TimedSettingModel(int settingId, string settingName, Type type,  DateTime start, DateTime end, object value)
        {
            SettingGroupId = settingId;
            Type = type;
            SettingName = settingName;
            Start = start;
            End = end;
            Value = value;
        }
    }
}
