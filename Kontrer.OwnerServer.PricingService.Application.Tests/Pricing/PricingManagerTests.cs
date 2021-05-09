using Bogus;
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
        [Fact]
        public async Task TestCalculate()
        {
            var mockRepo = new Mock<ISettingsRepository>();
            var dic = new Dictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>();
            IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>> parsedDic = (IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>)dic;
            mockRepo.Setup(x => x.GetScopedSettingsAsync(new List<SettingRequest>())).Returns(() => Task.FromResult(parsedDic));

            var mockUoW = new Mock<ISettingsUnitOfWork>();
            mockUoW.Setup(x => x.PricingSettingsRepository).Returns(() => mockRepo.Object);

            var mockUoWFactory = new Mock<IUnitOfWorkFactory<ISettingsUnitOfWork>>();
            mockUoWFactory.Setup(x => x.CreateUnitOfWork()).Returns(() => mockUoW.Object);
            var bp = BlueprintFakeData.GetAccommodationBlueprints(1)[0];

            Faker faker = new Faker();
            var refDate = faker.Date.Soon();
            var end = faker.Date.Soon(10, refDate);
            var start = faker.Date.Recent(10, refDate);            
                       
            
            var mockOptions = Options.Create<PricingManagerOptions>(new PricingManagerOptions() { });
            var pricingManager = new PricingManager(mockUoWFactory.Object, mockOptions,null,null);           
            var cost = await pricingManager.CalculateAccommodationCostAsync(bp);

            var mockBpEditor = new Mock<IAccommodationBlueprintEditor>();
            bool wasEditorRequiredCalled = false;
            mockBpEditor.Setup(x => x.GetRequiredSettings(bp)).Returns(() =>
            {
                wasEditorRequiredCalled = true;
                return new List<SettingRequest>() { new SettingRequest<int>("test1", start, end) };
            });
            bool wasEditCalled = false;
            mockBpEditor.Setup(x => x.EditBlueprint(It.IsAny<AccommodationBlueprint>(), It.IsAny<IScopedSettings>())).Callback(() => wasEditCalled = true);

            var mockPricer = new Mock<IAccommodationPricer>();
            bool wasPricerRequiredCalled = false;
            mockPricer.Setup((x) => x.GetRequiredSettings(bp)).Returns(() =>
            {
                wasPricerRequiredCalled = true;
                return new List<SettingRequest>() { new SettingRequest<int>("test1", start, end)}; 
            });
            bool wasCalculateCalled = false;
            mockPricer.Setup(x => x.CalculateContractCost(It.IsAny<AccommodationBlueprint>(), It.IsAny<RawAccommodationCost>(), It.IsAny<IScopedSettings>())).Callback(() => wasCalculateCalled = true);



            pricingManager = new PricingManager(mockUoWFactory.Object, mockOptions, new List<IAccommodationPricer>() { mockPricer.Object }, new List<IAccommodationBlueprintEditor>() { mockBpEditor.Object });
            cost = await pricingManager.CalculateAccommodationCostAsync(bp);

            Assert.True(wasEditorRequiredCalled);
            Assert.True(wasEditCalled);
            Assert.True(wasPricerRequiredCalled);
            Assert.True(wasCalculateCalled);
        }

      
    }
}
