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
        public TimeScope(int id)
        {
            Id = id;
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Name { get; set; }
        public int Id { get; init; }
        
    }
}
