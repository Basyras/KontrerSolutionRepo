using Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public class RawItemCost
    {
        public RawItemCost(ItemBlueprint blueprint)
        {
            Blueprint = blueprint;
        }

        public Dictionary<string, string> Manipulators { get; } = new();
        public decimal SubTotal { get; private set; } = 0;
        public string ItemName { get; }
        public ItemBlueprint Blueprint { get; }

        public void ManipulateCost(IPricingMiddleware pricingMiddleware, decimal newSubTotal)
        {
            ManipulateCost(pricingMiddleware.WorkDescription, newSubTotal, pricingMiddleware.GetType().Name);
        }

        public void ManipulateCost(string manipulationLog, decimal newSubTotal, [CallerFilePath] string manipulator = "")
        {
            manipulationLog = $"{manipulator}: {SubTotal} + {newSubTotal - SubTotal} = {newSubTotal} desc: {manipulationLog}";
            Manipulators.Add(manipulator, manipulationLog);
            SubTotal = newSubTotal;
        }
    }
}