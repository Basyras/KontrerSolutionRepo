using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.Abstraction.Pricing
{
    public class TimedSettingSelector
    {
        public TimedSettingSelector(string settingUniqueName, DateTime? start = null,DateTime? end = null)
        {
            SettingUniqueName = settingUniqueName;
            Start = start;
            End = end;
        }

        public string SettingUniqueName { get; }
        public DateTime? Start { get; }
        public DateTime? End { get; }
    }
}
