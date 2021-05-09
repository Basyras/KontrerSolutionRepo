using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    /// <summary>
    /// Base pricer class with method callbacks for each item
    /// </summary>
    public abstract class AccommodationPricerBase : IAccommodationPricer
    {
        public abstract int QueuePosition { get; }
        public abstract string WorkDescription { get; }
      

        
        public void CalculateContractCost(AccommodationBlueprint blueprint, RawAccommodationCost rawAccommodation, IScopedSettings resolver)
        {
            for (int i = 0; i < blueprint.AccommodationItems.Count; i++)
            {
                RawItemCost rawItemCost = rawAccommodation.RawAccommodationItems[i];
                ItemBlueprint itemBlueprint = blueprint.AccommodationItems[i];
                CallForEveryItem(itemBlueprint, rawItemCost,resolver);
            }

            for (int currentRoomIndex = 0; currentRoomIndex < blueprint.Rooms.Count; currentRoomIndex++)
            {
                RawRoomCost rawRoomCost = rawAccommodation.RawRooms[currentRoomIndex];
                RoomBlueprint roomBlueprint = blueprint.Rooms[currentRoomIndex];

                for (int currentRoomItemIndex = 0; currentRoomItemIndex < roomBlueprint.RoomItems.Count; currentRoomItemIndex++)
                {
                    RawItemCost rawItemCost = rawRoomCost.RawRoomItems[currentRoomItemIndex];
                    ItemBlueprint itemBlueprint = roomBlueprint.RoomItems[currentRoomItemIndex];
                    CallForEveryItem(itemBlueprint, rawItemCost,resolver);
                }


                for (int currentPersonIndex = 0; currentPersonIndex < roomBlueprint.People.Count; currentPersonIndex++)
                {
                    RawPersonCost rawPersonCost = rawRoomCost.RawPeople[currentPersonIndex];
                    PersonBlueprint personBlueprint = roomBlueprint.People[currentPersonIndex];


                    for (int currentPersonItemIndex = 0; currentPersonItemIndex < personBlueprint.PersonItems.Count; currentPersonItemIndex++)
                    {
                        RawItemCost rawPersonItemCost = rawPersonCost.RawPersonItems[currentPersonItemIndex];
                        ItemBlueprint personItemBlueprint = personBlueprint.PersonItems[currentPersonItemIndex];
                        CallForEveryItem(personItemBlueprint, rawPersonItemCost,resolver);
                    }

                }

                CallForEveryRoom(roomBlueprint, rawRoomCost,resolver);


            }
        }
        

        protected virtual void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, IScopedSettings settings)
        {
            
        }
     
        protected virtual void CallForEveryRoom(RoomBlueprint blueprint, RawRoomCost rawCost, IScopedSettings settings)
        {

        }

        public virtual List<SettingRequest> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            return null;
        }



    }
}
