using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Business.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;
using Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public class PricingManager : IPricingManager
    {
        private readonly IPricingSettingsRepository settingsRepository;
        private readonly IUnitOfWorkFactory<IPricingSettingsUnitOfWork> unitOfWorkFactory;
        private readonly IPricingSettingsResolver settingsResolver;
        private readonly IOptions<PriceManagerOptions> options;

        public PricingManager(IPricingSettingsRepository settingsRepository, IUnitOfWorkFactory<IPricingSettingsUnitOfWork> unitOfWorkFactory, IOptions<PriceManagerOptions> options)
        {
            this.settingsRepository = settingsRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.settingsResolver = settingsResolver;
            this.options = options;
        }



        public Task<AccommodationCost> CalculateAccommodationCost(AccommodationBlueprint accommodationBlueprint)
        {
            List<TimedSettingSelector> resolverRequests = new List<TimedSettingSelector>();
            foreach (IAccommodationBlueprintEditor editor in options.Value.AccommodationEditors)
            {
                resolverRequests.AddRange(editor.GetRequiredSettings(accommodationBlueprint));
            }

            foreach (IAccommodationPricingMiddleware pricer in options.Value.AccommodationPricers.OrderBy(x => x.QueuePosition))
            {
                resolverRequests.AddRange(pricer.GetRequiredSettings(accommodationBlueprint));
            }



            foreach (IAccommodationBlueprintEditor editor in options.Value.AccommodationEditors)
            {
                editor.EditBlueprint(accommodationBlueprint, settingsResolver);
            }

            RawAccommodationCost rawAccoCost = PrepareRawCost(accommodationBlueprint);

            foreach (IAccommodationPricingMiddleware pricer in options.Value.AccommodationPricers.OrderBy(x=>x.QueuePosition))
            {
                pricer.CalculateContractCost(accommodationBlueprint, rawAccoCost, settingsResolver);
            }
            
            AccommodationCost accommodationCost = FinishAccommodationCost(accommodationBlueprint.Currency, rawAccoCost);

            return Task.FromResult(accommodationCost);
        }

        private RawAccommodationCost PrepareRawCost(AccommodationBlueprint blueprint)
        {
            var rawItems = blueprint.AccommodationItems.Select(accoItemBp => new RawItemCost(accoItemBp)).ToList();
            var rawRooms = blueprint.Rooms.Select(roomBp => new RawRoomCost(roomBp.RoomItems.Select(roomItemBp => new RawItemCost(roomItemBp)).ToList(), roomBp.People.Select(peopleBp => new RawPersonCost()).ToList())).ToList();
            RawAccommodationCost rawAccoCost = new RawAccommodationCost(blueprint.Currency, rawItems, rawRooms);
            return rawAccoCost;
        }

        private AccommodationCost FinishAccommodationCost(Currencies currency, RawAccommodationCost rawAccommodationCost)
        {
            List<ItemCost> accoItemCosts = rawAccommodationCost.RawAccommodationItems.Select(x => FinishItemCost(currency, x)).ToList();
            List<RoomCost> roomCosts = rawAccommodationCost.RawRooms.Select(x => FinishRoomCost(currency, x)).ToList();
            decimal totalAmount = accoItemCosts.Sum(x => x.TotalCost.Amout) + roomCosts.Sum(x => x.TotalCost.Amout);
            Cash totalCash = new Cash(currency, totalAmount);
            AccommodationCost accommodationCost = new AccommodationCost(roomCosts.AsReadOnly(), accoItemCosts.AsReadOnly(), totalCash);
            return accommodationCost;
        }

        private ItemCost FinishItemCost(Currencies currency, RawItemCost rawItemCost)
        {
            var itemCost = new ItemCost(rawItemCost.Blueprint.ItemName, rawItemCost.Blueprint.ExtraInfo, rawItemCost.Blueprint.CostPerOne, rawItemCost.Blueprint.Count, new Cash(rawItemCost.Blueprint.CostPerOne.Currency, rawItemCost.SubTotal));
            return itemCost;
        }

        private PersonCost FinishPersonCost(Currencies currency, RawPersonCost rawPersonCost)
        {
            var totalCash = new Cash(currency, rawPersonCost.RawPersonItems.Sum(x => x.SubTotal));
            var personCost = new PersonCost(rawPersonCost.RawPersonItems.Select(x => FinishItemCost(currency, x)).ToList().AsReadOnly(), totalCash);
            return personCost;
        }


        private RoomCost FinishRoomCost(Currencies currency, RawRoomCost rawRoomCost)
        {
            List<ItemCost> roomItemCosts = rawRoomCost.RawRoomItems.Select(x => FinishItemCost(currency, x)).ToList();
            List<PersonCost> peopleCosts = rawRoomCost.RawPeople.Select(x => FinishPersonCost(currency, x)).ToList();
            decimal totalAmount = roomItemCosts.Sum(x => x.TotalCost.Amout) + peopleCosts.Sum(x => x.TotalCost.Amout);
            Cash totalCash = new Cash(currency, totalAmount);
            RoomCost roomCost = new RoomCost(peopleCosts.AsReadOnly(), roomItemCosts.AsReadOnly(), totalCash);
            return roomCost;
        }

        public IPricingSettingsUnitOfWork CreatePricingSettingsUnitOfWork()
        {
            return unitOfWorkFactory.CreateUnitOfWork();
        }
    }
}
