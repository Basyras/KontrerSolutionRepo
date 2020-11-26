using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.Shared.Models.Pricing.Costs
{
    public record ItemCost(string Name, Dictionary<string,string> Descriptions,Cash CostPerOne,int Count, Cash TotalCost);
    
    
}
