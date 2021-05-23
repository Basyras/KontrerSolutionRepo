using Bogus;
using FluentAssertions;
using Kontrer.OwnerServer.PricingService.Application.Processing;
using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Kontrer.Shared.Tests.FakeData;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.PricingService.Application.Tests.Pricing
{
    public class PricingManagerTests
    {
        private readonly Mock<ISettingsRepository> mockSettingsRepo;
        private const string testSetting1Name = "TestSetting1";
        private readonly Faker faker;        
        private readonly DateTime refDate;
        private readonly DateTime contractEnd;
        private readonly DateTime contractStart;
        private readonly string TestScope1 = "TestScope1";

        public PricingManagerTests()
        {
            faker = new Faker();
            mockSettingsRepo = new Mock<ISettingsRepository>();
            var settings = (IDictionary<string, NullableResult<object>>)new Dictionary<string, NullableResult<object>>();
            mockSettingsRepo.Setup(x => x.GetScopedSettingsAsync(It.IsAny<List<ScopedSettingRequest>>())).Returns(() => Task.FromResult(settings));
            mockSettingsRepo.Setup(x => x.GetTimeScopesAsync()).Returns(() => Task.FromResult(new List<TimeScope>()
            {
                new TimeScope(){ From =DateTime.MinValue, To = DateTime.MaxValue, Id = 1, Name = TestScope1  }
            }));

           
            refDate = faker.Date.Soon();
            contractEnd = faker.Date.Soon(10, refDate);
            contractStart = faker.Date.Recent(10, refDate);
        }

        [Fact]
        public async Task Should_Calculate_Without_Editors_And_Blueprints()
        {
            var mockOptions = Options.Create<PricingManagerOptions>(new PricingManagerOptions() { });
            AccommodationBlueprint accoBlueprint = BlueprintFakeData.GetAccommodationBlueprints(1)[0];

            var pricingManager = new PricingManager(mockSettingsRepo.Object, mockOptions, null, null);
            var cost = await pricingManager.CalculateAccommodationCostAsync(accoBlueprint);
            Assert.NotNull(cost);
        }

        [Fact]
        public async Task Should_Apply_Editor()
        {
            const string editorItem = "editorItem";

            var mockBpEditor = new Mock<IAccommodationBlueprintEditor>();
            bool wasEditorRequiredCalled = false;
            mockBpEditor.Setup(x => x.GetRequiredSettings(It.IsAny<AccommodationBlueprint>()))
                .Returns(() =>
                {
                    wasEditorRequiredCalled = true;
                    return new List<SettingRequest>() { new SettingRequest<int>(testSetting1Name) };
                });
            bool wasEditCalled = false;
            mockBpEditor.Setup(x => x.EditBlueprint(It.IsAny<AccommodationBlueprint>(), It.IsAny<IResolvedScopedSettings>()))
                .Callback((AccommodationBlueprint blueprint, IResolvedScopedSettings settings) =>
                {
                    wasEditCalled = true;
                    blueprint.AccommodationItems.Add(new ItemBlueprint(new Kontrer.Shared.Models.Pricing.Cash(blueprint.Currency, 0), 1, 15, editorItem));
                });

            var mockOptions = Options.Create(new PricingManagerOptions());
            var pricingManager = new PricingManager(mockSettingsRepo.Object, mockOptions, Enumerable.Empty<IAccommodationPricer>(), new List<IAccommodationBlueprintEditor>() { mockBpEditor.Object });
            AccommodationBlueprint accoBlueprint = BlueprintFakeData.GetAccommodationBlueprints(1)[0];
            var cost = await pricingManager.CalculateAccommodationCostAsync(accoBlueprint);


            Assert.True(wasEditorRequiredCalled, "Price manager didn't ask for editors settings");
            Assert.True(wasEditCalled, "Price manager didn't call editor");
            cost.AccomodationItems.Should().Contain(x => x.Name == editorItem, "Editor's changes didn't persist");

        }

        [Fact]
        public async Task Should_Call_Pricer()
        {
            const string newPricerItemName = "PricerItemName";
            var mockPricer = new Mock<IAccommodationPricer>();
            bool wasPricerRequiredCalled = false;
            mockPricer.Setup(x => x.GetRequiredSettings(It.IsAny<AccommodationBlueprint>())).Returns(() =>
            {
                wasPricerRequiredCalled = true;
                return new List<SettingRequest>() { new SettingRequest<int>(testSetting1Name) };
            });
            bool wasCalculateCalled = false;
            mockPricer.Setup(x => x.CalculateContractCost(It.IsAny<AccommodationBlueprint>(), It.IsAny<RawAccommodationCost>(), It.IsAny<IResolvedScopedSettings>())).Callback((AccommodationBlueprint bp,RawAccommodationCost rawCost,IResolvedScopedSettings settings) =>
            {
                wasCalculateCalled = true;
                var newRawItemCost = new RawItemCost(new ItemBlueprint(new Cash(Currencies.EUR,0),1,0, newPricerItemName) );
                newRawItemCost.ManipulateCost("addint new cost item",0,"Mock pricer");
                rawCost.RawAccommodationItems.Add(newRawItemCost);
            });

            var mockOptions = Options.Create<PricingManagerOptions>(new PricingManagerOptions());
            var pricingManager = new PricingManager(mockSettingsRepo.Object, mockOptions, new List<IAccommodationPricer>() { mockPricer.Object }, Enumerable.Empty<IAccommodationBlueprintEditor>());
            AccommodationBlueprint accoBlueprint = BlueprintFakeData.GetAccommodationBlueprints(1)[0];
            var cost = await pricingManager.CalculateAccommodationCostAsync(accoBlueprint);
            wasPricerRequiredCalled.Should().BeTrue();
            wasCalculateCalled.Should().BeTrue();
            cost.AccomodationItems.Should().Contain(x => x.Name == newPricerItemName);


        }



    }
}
