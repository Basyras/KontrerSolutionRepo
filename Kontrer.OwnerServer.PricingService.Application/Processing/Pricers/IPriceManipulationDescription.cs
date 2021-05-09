using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    public interface IPriceManipulationDescription
    {
        /// <summary>
        /// 0 - does not have priority, higher = executed later
        /// </summary>
        int QueuePosition { get; }
        string WorkDescription { get; }
    }
}
