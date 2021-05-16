using Bogus;
using FluentAssertions;
using Kontrer.OwnerServer.PricingService.Application.Processing;
using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
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
        private readonly Mock<ISettingsUnitOfWork> mockUoW;
        private const string settingTest1 = "test1";
        private readonly Faker faker;
        private readonly Mock<IUnitOfWorkFactory<ISettingsUnitOfWork>> mockUoWFactory;

        public PricingManagerTests()
        {            
            faker = new Faker();
            mockSettingsRepo = new Mock<ISettingsRepository>();
            var settings = (IDictionary<string, NullableResult<object>>)new Dictionary<string, NullableResult<object>>();
            mockSettingsRepo.Setup(x => x.GetScopedSettingsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SettingRequest>>())).Returns(() => Task.FromResult(settings));
            mockUoW = new Mock<ISettingsUnitOfWork>();
            mockUoW.Setup(x => x.PricingSettingsRepository).Returns(() => mockSettingsRepo.Object);

            mockUoWFactory = new Mock<IUnitOfWorkFactory<ISettingsUnitOfWork>>();
            mockUoWFactory.Setup(x => x.CreateUnitOfWork()).Returns(() => mockUoW.Object);
        }

        [Fact]
        public async Task Should_Calculate_Without_Editors_And_Blueprints()
        {   
            var mockOptions = Options.Create<PricingManagerOptions>(new PricingManagerOptions() { });
            AccommodationBlueprint accoBlueprint = BlueprintFakeData.GetAccommodationBlueprints(1)[0];           
     
            var pricingManager = new PricingManager(mockUoWFactory.Object, mockOptions, null, null);
            var cost = await pricingManager.CalculateAccommodationCostAsync(accoBlueprint);
            Assert.NotNull(cost);
        }

        [Fact]
        public async Task Should_Apply_Editor()
        {
            var refDate = faker.Date.Soon();
            var contractEnd = faker.Date.Soon(10, refDate);
            var contractStart = faker.Date.Recent(10, refDate);
            const string editorItem = "editorItem";

            var mockBpEditor = new Mock<IAccommodationBlueprintEditor>();
            bool wasEditorRequiredCalled = false;
            mockBpEditor.Setup(x => x.GetRequiredSettings(It.IsAny<AccommodationBlueprint>()))
                .Returns(() =>
                {
                    wasEditorRequiredCalled = true;
                    return new List<SettingRequest>() { new SettingRequest<int>(settingTest1) };
                });
            bool wasEditCalled = false;
            mockBpEditor.Setup(x => x.EditBlueprint(It.IsAny<AccommodationBlueprint>(), It.IsAny<IScopedSettings>()))
                .Callback((AccommodationBlueprint blueprint, IScopedSettings settings) => 
                {
                    wasEditCalled = true;           
                    blueprint.AccommodationItems.Add(new ItemBlueprint(new Kontrer.Shared.Models.Pricing.Cash(blueprint.Currency,0),1,15, editorItem));
                });

            var mockOptions = Options.Create(new PricingManagerOptions());
            var pricingManager = new PricingManager(mockUoWFactory.Object, mockOptions,Enumerable.Empty<IAccommodationPricer>(), new List<IAccommodationBlueprintEditor>() { mockBpEditor.Object });
            AccommodationBlueprint accoBlueprint = BlueprintFakeData.GetAccommodationBlueprints(1)[0];
            var cost = await pricingManager.CalculateAccommodationCostAsync(accoBlueprint);


            Assert.True(wasEditorRequiredCalled,"Price manager didn't ask for editors settings");
            Assert.True(wasEditCalled,"Price manager didn't call editor");
            cost.AccomodationItems.Should().Contain(x => x.Name == editorItem,"Editor's changes didn't persist");
      
        }

        [Fact]
        public async Task Should_Call_Pricer()
        {
            var mockPricer = new Mock<IAccommodationPricer>();
            bool wasPricerRequiredCalled = false;
            mockPricer.Setup(x => x.GetRequiredSettings(It.IsAny<AccommodationBlueprint>())).Returns(() =>
            {
                wasPricerRequiredCalled = true;
                return new List<SettingRequest>() { new SettingRequest<int>(settingTest1) };
            });
            bool wasCalculateCalled = false;
            mockPricer.Setup(x => x.CalculateContractCost(It.IsAny<AccommodationBlueprint>(), It.IsAny<RawAccommodationCost>(), It.IsAny<IScopedSettings>())).Callback(() => wasCalculateCalled = true);

            var mockOptions = Options.Create<PricingManagerOptions>(new PricingManagerOptions());
            var pricingManager = new PricingManager(mockUoWFactory.Object, mockOptions, new List<IAccommodationPricer>() { mockPricer.Object },Enumerable.Empty<IAccommodationBlueprintEditor>());
            AccommodationBlueprint accoBlueprint = BlueprintFakeData.GetAccommodationBlueprints(1)[0];
            _ = await pricingManager.CalculateAccommodationCostAsync(accoBlueprint);

            wasPricerRequiredCalled.Should().BeTrue();
            wasCalculateCalled.Should().BeTrue();
            
            
        }



    }
}
