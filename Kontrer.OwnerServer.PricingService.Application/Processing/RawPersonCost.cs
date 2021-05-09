using System.Collections.Generic;

namespace Kontrer.OwnerServer.PricingService.Application.Processing
{
    public class RawPersonCost
    {
        public RawPersonCost()
        {

        }
        public RawPersonCost(List<RawItemCost> rawPersonItems)
        {
            RawPersonItems = rawPersonItems;
        }
        public List<RawItemCost> RawPersonItems { get; set; } = new List<RawItemCost>();
    }
}