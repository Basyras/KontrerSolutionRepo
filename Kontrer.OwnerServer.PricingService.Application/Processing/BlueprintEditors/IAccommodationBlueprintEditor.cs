using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors
{
    /// <summary>
    /// Prepares accommodation blueprint before calculating the price. Settings default values, discounts, etc.
    /// </summary>
    public interface IAccommodationBlueprintEditor : IBlueprintEditor<AccommodationBlueprint> 
    {
      
    }
}
