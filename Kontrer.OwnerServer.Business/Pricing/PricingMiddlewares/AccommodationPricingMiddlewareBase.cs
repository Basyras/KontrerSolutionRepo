﻿using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public abstract class AccommodationPricingMiddlewareBase : IPricingMiddleware<AccommodationBlueprint, RawAccommodationCost>
    {
        //protected const string RoomGroup = "RoomGroup";
        public abstract int QueuePosition { get; }
        public abstract string WorkDescription { get; }

        //Dictionary<string, Action<ItemBlueprint, RawItemCost>> Calls = new Dictionary<string, Action<ItemBlueprint, RawItemCost>>();
        //Action<ItemBlueprint, RawItemCost> EveryItemAction = null

        
        public void CalculateContractCost(AccommodationBlueprint blueprint, RawAccommodationCost rawAccommodation, ITimedSettingResolver resolver)
        {
            for (int i = 0; i < blueprint.AccommodationItems.Count; i++)
            {
                RawItemCost rawItemCost = rawAccommodation.RawAccommodationItems[i];
                ItemBlueprint itemBlueprint = blueprint.AccommodationItems[i];
                CallForEveryItem(itemBlueprint, rawItemCost,resolver);
                //Calls[RoomGroup](itemBlueprint, rawItemCost);
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
                    //Calls[RoomGroup](itemBlueprint, rawItemCost);
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
                        //Calls[RoomGroup](personItemBlueprint, rawPersonItemCost);
                    }

                }

                CallForEveryRoom(roomBlueprint, rawRoomCost,resolver);
                //Calls[RoomGroup](roomBlueprint, rawRoomCost);


            }
        }

     

        public List<TimedSettingSelector> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            throw new NotImplementedException();
        }

        protected virtual void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, ITimedSettingResolver resolver)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blueprint"></param>
        /// <param name="rawCost"></param>
        /// <param name="resolver"></param>
        protected virtual void CallForEveryRoom(RoomBlueprint blueprint, RawRoomCost rawCost, ITimedSettingResolver resolver)
        {

        }

        //protected void AddCallForEveryItem(string groupName,Action<ItemBlueprint, RawItemCost> func)
        //{
        //    Calls.Add(groupName, func);
        //}



    }
}