using Kontrer.OwnerServer.PricingService.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.PricingService.Business.Pricing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Business.Pricing.PricingMiddlewares;
using Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing;
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

namespace Kontrer.OwnerServer.PricingService.Business.Pricing
{
    public class PricingManager : IPricingManager
    {
        
        private readonly IUnitOfWorkFactory<IPricingSettingsUnitOfWork> unitOfWorkFactory;
        private readonly IOptions<PricingManagerOptions> options;
        private readonly List<IAccommodationPricer> accommodationPricers = new List<IAccommodationPricer>();
        private readonly List<IAccommodationBlueprintEditor> accommodationEditors = new List<IAccommodationBlueprintEditor>();

        public PricingManager(IUnitOfWorkFactory<IPricingSettingsUnitOfWork> unitOfWorkFactory, IOptions<PricingManagerOptions> options, IEnumerable<IAccommodationPricer> accommodationPricers, IEnumerable<IAccommodationBlueprintEditor> accommodationEditors)
        {            
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.options = options;
            if(accommodationPricers != null)
            this.accommodationPricers = new List<IAccommodationPricer>(accommodationPricers);
            if (accommodationEditors != null)
                this.accommodationEditors = new List<IAccommodationBlueprintEditor>(accommodationEditors);
        }


        private async Task<ITimedSettingResolver> GetResolverForBlueprintAsync(AccommodationBlueprint accommodationBlueprint)
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

            foreach (IAccommodationPricer pricer in accommodationPricers.OrderBy(x => x.QueuePosition))
            {
                var required = pricer.GetRequiredSettings(accommodationBlueprint);
                if (required != null && required.Count > 0)
                {
                    settingRequests.AddRange(required);
                }
            }

            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>> requiredSettings = await unitOfWork.PricingSettingsRepository.GetTimedSettingsAsync(settingRequests);
            TimedSettingResolver settingsResolver = new TimedSettingResolver(requiredSettings);

            return settingsResolver;
        }
        public async Task<AccommodationCost> CalculateAccommodationCostAsync(AccommodationBlueprint accommodationBlueprint)
        {
            ITimedSettingResolver settingsResolver = await GetResolverForBlueprintAsync(accommodationBlueprint);

            foreach (IAccommodationBlueprintEditor editor in accommodationEditors)
            {
                editor.EditBlueprint(accommodationBlueprint, settingsResolver);
            }

            RawAccommodationCost rawAccoCost = PrepareRawCost(accommodationBlueprint);

            foreach (IAccommodationPricer pricer in accommodationPricers.OrderBy(x => x.QueuePosition))
            {
                pricer.CalculateContractCost(accommodationBlueprint, rawAccoCost, settingsResolver);
            }

            AccommodationCost accommodationCost = FinishAccommodationCost(accommodationBlueprint.Currency, rawAccoCost);

            return accommodationCost;
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
