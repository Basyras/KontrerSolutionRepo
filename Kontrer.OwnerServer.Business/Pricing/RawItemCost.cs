using Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System.Collections.Generic;

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

        public void Manipulate(string manipulator, string manipulationLog, decimal newSubTotal)
        {
            manipulationLog = $"{manipulator}: {SubTotal} + {newSubTotal - SubTotal} = {newSubTotal} desc: {manipulationLog}";
            Manipulators.Add(manipulator, manipulationLog);
            SubTotal = newSubTotal;
        }
    }
}