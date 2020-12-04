using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
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
        
        private readonly IUnitOfWorkFactory<IPricingSettingsUnitOfWork> unitOfWorkFactory;
        private readonly IOptions<PricingManagerOptions> options;
        private readonly List<IAccommodationPricingMiddleware> accommodationPricers = new List<IAccommodationPricingMiddleware>();
        private readonly List<IAccommodationBlueprintEditor> accommodationEditors = new List<IAccommodationBlueprintEditor>();

        public PricingManager(IUnitOfWorkFactory<IPricingSettingsUnitOfWork> unitOfWorkFactory, IOptions<PricingManagerOptions> options, IEnumerable<IAccommodationPricingMiddleware> accommodationPricers, IEnumerable<IAccommodationBlueprintEditor> accommodationEditors)
        {            
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.options = options;
            if(accommodationPricers != null)
            this.accommodationPricers = new List<IAccommodationPricingMiddleware>(accommodationPricers);
            if (accommodationEditors != null)
                this.accommodationEditors = new List<IAccommodationBlueprintEditor>(accommodationEditors);
        }


        private ITimedSettingResolver GetResolverForBlueprint(AccommodationBlueprint accommodationBlueprint)
        {
            List<TimedSettingSelector> settingRequests = new List<TimedSettingSelector>();

            foreach (IAccommodationBlueprintEditor editor in accommodationEditors)
            {
                var required = editor.GetRequiredSettings(accommodationBlueprint);
                if (required != null && required.Count > 0)
                {
                    settingRequests.AddRange(required);
                }
            }

            foreach (IAccommodationPricingMiddleware pricer in accommodationPricers.OrderBy(x => x.QueuePosition))
            {
                var required = pricer.GetRequiredSettings(accommodationBlueprint);
                if (required != null && required.Count > 0)
                {
                    settingRequests.AddRange(required);
                }
            }

            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            IDictionary<string, NullableResult<object>> requiredSettings = unitOfWork.PricingSettingsRepository.GetTimedSettings(settingRequests);
            TimedSettingResolver settingsResolver = new TimedSettingResolver(requiredSettings);

            return settingsResolver;
        }
        public Task<AccommodationCost> CalculateAccommodationCost(AccommodationBlueprint accommodationBlueprint)
        {
            ITimedSettingResolver settingsResolver = GetResolverForBlueprint(accommodationBlueprint);

            foreach (IAccommodationBlueprintEditor editor in accommodationEditors)
            {
                editor.EditBlueprint(accommodationBlueprint, settingsResolver);
            }

            RawAccommodationCost rawAccoCost = PrepareRawCost(accommodationBlueprint);

            foreach (IAccommodationPricingMiddleware pricer in accommodationPricers.OrderBy(x => x.QueuePosition))
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
            decimal totalCost = (rawPersonCost.RawPersonItems.Count > 0) ?  rawPersonCost.RawPersonItems.Sum(x => x.SubTotal) : 0m;
            Cash totalCash = new Cash(currency, totalCost);
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
