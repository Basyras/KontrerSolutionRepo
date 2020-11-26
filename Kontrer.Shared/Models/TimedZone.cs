using Kontrer.Shared.Models.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models
{
    public class TimedZone
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string TimeZoneName { get; set; }
        public int Id { get; set; }
        public List<TimedPricingSetting> TimedSettings { get; set; }

        public bool IsInZone(DateTime zoneStart, DateTime zoneEnd)
        {

            if (Start.Date.DayOfYear > zoneStart.Date.DayOfYear)
            {
                return false;
            }

            if (Start.Date.DayOfYear > zoneEnd.Date.DayOfYear)
            {
                return false;
            }

            if (End.Date.DayOfYear < zoneStart.Date.DayOfYear)
            {
                return false;
            }

            if (End.Date.DayOfYear < zoneEnd.Date.DayOfYear)
            {
                return false;
            }
            return true;
        }
    }
}
