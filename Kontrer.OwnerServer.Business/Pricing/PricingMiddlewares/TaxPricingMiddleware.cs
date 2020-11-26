using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    /// <summary>
    /// Must be called as last in pricing pipeline
    /// </summary>
    public class TaxPricingMiddleware : IAccommodationPricingMiddleware
    {
        public string WorkDescription => "Adding taxes to cost";

        public void CalculateContractCost(AccommodationBlueprint blueprint, ref RawAccommodationCostModel rawAccommodation, IPricingSettingsResolver resolver)
        {

            // foreach (RawItemCost roomItem in rawAccommodation.RawAccommodationItems)
            for (int i = 0; i < blueprint.AccommodationItems.Count; i++)
            {
                RawItemCost rawItemCost = rawAccommodation.RawAccommodationItems[i];
                ItemBlueprint itemBlueprint = blueprint.AccommodationItems[i];
                decimal taxAmountToAdd = rawItemCost.SubTotal * (decimal)itemBlueprint.TaxPercentageToAdd;
                decimal newSubTotal = rawItemCost.SubTotal + taxAmountToAdd;
                //rawRoomCost.Manipulate(nameof(TaxPricingMiddleware), $"Adding tax of {roomBlueprint.TaxPercentageToAdd}% resulting in {taxAmountToAdd} {roomBlueprint.CostPerOne.Currency}, new subTotal {newSubTotal} {roomBlueprint.CostPerOne.Currency}", newSubTotal);
                rawItemCost.Manipulate(nameof(TaxPricingMiddleware), $"Adding tax",newSubTotal);
            }

            for (int currentRoomIndex = 0; currentRoomIndex < blueprint.Rooms.Count; currentRoomIndex++)
            {
                RawRoomCost rawRoomCost = rawAccommodation.RawRooms[currentRoomIndex];
                RoomBlueprint roomBlueprint = blueprint.Rooms[currentRoomIndex];
                for (int currentPersonIndex = 0; currentPersonIndex < roomBlueprint.People.Count; currentPersonIndex++)
                {
                    RawPersonCost rawPersonCost = rawRoomCost.RawPeople[currentPersonIndex];
                    PersonBlueprint personBlueprint = roomBlueprint.People[currentPersonIndex];


                    for (int currentPersonItemIndex = 0; currentPersonItemIndex < personBlueprint.PersonItems.Count; currentPersonItemIndex++)
                    {
                        RawItemCost rawPersonItemCost = rawPersonCost.RawPersonItems[currentPersonItemIndex];
                        ItemBlueprint personItemBlueprint = personBlueprint.PersonItems[currentPersonItemIndex];
                        decimal taxAmountToAdd = rawPersonItemCost.SubTotal * (decimal)personItemBlueprint.TaxPercentageToAdd;
                        decimal newSubTotal = rawPersonItemCost.SubTotal + taxAmountToAdd;
                        rawPersonItemCost.Manipulate(nameof(TaxPricingMiddleware), $"Adding tax", newSubTotal);
                    }
                
                }
              
            }
        }
    }
}
