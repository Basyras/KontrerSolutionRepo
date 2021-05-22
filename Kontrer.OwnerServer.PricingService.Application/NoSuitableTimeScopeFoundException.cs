using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application
{
    public class NoSuitableTimeScopeFoundException : Exception
    {
        public NoSuitableTimeScopeFoundException(DateTime from, DateTime to) : base($"No suitable time scope found. Request range from: {from.ToShortDateString()} to: {to.ToShortDateString()}")
        {

}
        }
    }
