using Basyc.Shared.Models.Pricing;
using Basyc.Shared.Models.Pricing.Costs;
using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using Kontrer.OwnerServer.PricingService.Application.Processing;
using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application
{
    public class PricingManager
    {
        public ISettingsRepository SettingRepository { get; }
        private readonly IOptions<PricingManagerOptions> options;
        private readonly List<IAccommodationPricer> accommodationPricers = new List<IAccommodationPricer>();
        private readonly List<IAccommodationBlueprintEditor> accommodationEditors = new List<IAccommodationBlueprintEditor>();

        public PricingManager(ISettingsRepository settingRepository, IOptions<PricingManagerOptions> options, IEnumerable<IAccommodationPricer> accommodationPricers, IEnumerable<IAccommodationBlueprintEditor> accommodationEditors)
        {
            this.SettingRepository = settingRepository;
            this.options = options;
            if (accommodationPricers != null)
                this.accommodationPricers = new List<IAccommodationPricer>(accommodationPricers);
            if (accommodationEditors != null)
                this.accommodationEditors = new List<IAccommodationBlueprintEditor>(accommodationEditors);
        }

        public async Task<AccommodationCost> CalculateAccommodationCostAsync(AccommodationBlueprint accommodationBlueprint)
        {
            IResolvedScopedSettings settings = await GetSettingsCacheAsync(accommodationBlueprint);

            foreach (IAccommodationBlueprintEditor editor in accommodationEditors)
            {
                editor.EditBlueprint(accommodationBlueprint, settings);
            }

            RawAccommodationCost rawAccoCost = PrepareRawCost(accommodationBlueprint);

            foreach (IAccommodationPricer pricer in accommodationPricers.OrderBy(x => x.QueuePosition))
            {
                pricer.CalculateContractCost(accommodationBlueprint, rawAccoCost, settings);
            }

            AccommodationCost accommodationCost = FinishAccommodationCost(accommodationBlueprint.Currency, rawAccoCost);

            return accommodationCost;
        }

        private async Task<IResolvedScopedSettings> GetSettingsCacheAsync(AccommodationBlueprint accommodationBlueprint)
        {
            //TODO can implement caching, or actor model?
            List<SettingRequest> settingRequests = new List<SettingRequest>();

            foreach (IAccommodationBlueprintEditor editor in accommodationEditors)
            {
                var settingRequest = editor.GetRequiredSettings(accommodationBlueprint);
                if (settingRequest != null && settingRequest.Count > 0)
                {
                    settingRequests.AddRange(settingRequest);
                }
            }

            foreach (IAccommodationPricer pricer in accommodationPricers.OrderBy(x => x.QueuePosition))
            {
                List<SettingRequest> required = pricer.GetRequiredSettings(accommodationBlueprint);
                if (required != null && required.Count > 0)
                {
                    settingRequests.AddRange(required);
                }
            }
            var scopedRequestsTasks = settingRequests.Select(x => GetBestScopedSettingRequest(x, SettingRepository, accommodationBlueprint.From, accommodationBlueprint.To)).ToList();
            var scopedRequests = await Task.WhenAll(scopedRequestsTasks);
            var scopedSettings = await SettingRepository.GetScopedSettingsAsync(scopedRequests);
            InMemoryResolvedScopedSettings settingsResolver = new InMemoryResolvedScopedSettings(accommodationBlueprint.From, accommodationBlueprint.To, scopedSettings);

            return settingsResolver;
        }

        /// <summary>
        /// Creates empty PriceItemCosts for pricers
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        private RawAccommodationCost PrepareRawCost(AccommodationBlueprint blueprint)
        {
            var rawItems = blueprint.AccommodationItems.Select(accoItemBp => new RawItemCost(accoItemBp)).ToList();
            var rawRooms = blueprint.Rooms.Select(roomBp => new RawRoomCost(roomBp.RoomItems.Select(roomItemBp => new RawItemCost(roomItemBp)).ToList(), roomBp.People.Select(peopleBp => new RawPersonCost()).ToList())).ToList();
            RawAccommodationCost rawAccoCost = new RawAccommodationCost(blueprint.Currency, rawItems, rawRooms);
            return rawAccoCost;
        }

        /// <summary>
        /// Sum up all rawcosts into final costs
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="rawAccommodationCost"></param>
        /// <returns></returns>
        private AccommodationCost FinishAccommodationCost(Currencies currency, RawAccommodationCost rawAccommodationCost)
        {
            List<ItemCost> accoItemCosts = rawAccommodationCost.RawAccommodationItems.Select(x => FinishItemCost(currency, x)).ToList();
            List<RoomCost> roomCosts = rawAccommodationCost.RawRooms.Select(x => FinishRoomCost(currency, x)).ToList();
            decimal totalAmount = accoItemCosts.Sum(x => x.TotalCost.Amount) + roomCosts.Sum(x => x.TotalCost.Amount);
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
            decimal totalCost = (rawPersonCost.RawPersonItems.Count > 0) ? rawPersonCost.RawPersonItems.Sum(x => x.SubTotal) : 0m;
            Cash totalCash = new Cash(currency, totalCost);
            var personCost = new PersonCost(rawPersonCost.RawPersonItems.Select(x => FinishItemCost(currency, x)).ToList().AsReadOnly(), totalCash);
            return personCost;
        }

        private RoomCost FinishRoomCost(Currencies currency, RawRoomCost rawRoomCost)
        {
            List<ItemCost> roomItemCosts = rawRoomCost.RawRoomItems.Select(x => FinishItemCost(currency, x)).ToList();
            List<PersonCost> peopleCosts = rawRoomCost.RawPeople.Select(x => FinishPersonCost(currency, x)).ToList();
            decimal totalAmount = roomItemCosts.Sum(x => x.TotalCost.Amount) + peopleCosts.Sum(x => x.TotalCost.Amount);
            Cash totalCash = new Cash(currency, totalAmount);
            RoomCost roomCost = new RoomCost(peopleCosts.AsReadOnly(), roomItemCosts.AsReadOnly(), totalCash);
            return roomCost;
        }

        private async Task<ScopedSettingRequest> GetBestScopedSettingRequest(SettingRequest request, ISettingsRepository settingsRepository, DateTime from, DateTime to)
        {
            var allScopes = await settingsRepository.GetTimeScopesAsync();
            var scopesCandidates = allScopes.Where(scope => scope.From <= from && scope.To >= to).ToList();
            TimeScope finalScope = null;
            if (scopesCandidates.Count > 0)
            {
                finalScope = scopesCandidates.OrderBy(scope => Math.Abs((scope.From - from).TotalDays)).First();
            }
            else
            {
                scopesCandidates = scopesCandidates.Where(scope => scope.From.Month <= from.Month && scope.To.Month >= to.Month).ToList();
                if (scopesCandidates.Count > 0)
                {
                    finalScope = scopesCandidates.OrderBy(scope => Math.Abs((scope.From.Month - from.Month))).ThenByDescending(x => x.From.Year).First();
                }
                else
                {
                    throw new NoSuitableTimeScopeFoundException(from, to);
                }
            }

            return new ScopedSettingRequest(request, finalScope.Id);
        }
    }
}