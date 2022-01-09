using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kontrer.OwnerServer.PricingService.Application.Processing
{
    public class RawItemCost
    {
        public RawItemCost(ItemBlueprint blueprint)
        {
            Blueprint = blueprint;
        }

        public string ItemName { get; }
        public ItemBlueprint Blueprint { get; }
        /// <summary>
        /// Contains logs about every price manipulation
        /// </summary>
        public List<string> ManipulatorLog { get; } = new();
        public decimal SubTotal { get; private set; } = 0;

        public void ManipulateCost(IPriceManipulationDescription description, decimal newSubTotal)
        {
            ManipulateCost(description.WorkDescription, newSubTotal, description.GetType().Name);
        }

        public void ManipulateCost(string manipulationLog, decimal newSubTotal, [CallerFilePath] string manipulatorName = "")
        {
            manipulationLog = $"{manipulatorName}: {SubTotal} -> {newSubTotal} desc: {manipulationLog}";
            ManipulatorLog.Add(manipulationLog);
            SubTotal = newSubTotal;
        }
    }
}