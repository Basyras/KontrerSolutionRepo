using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{
    public class TimeScope
    {      

        public TimeScope()
        {

        }

        public DateTime From { get; }
        public DateTime To { get; }
        public string Name { get; }
        public int Id { get; }
    }
}
