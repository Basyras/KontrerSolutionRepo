using Bogus;
using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Business.Pricing;
using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
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

namespace Kontrer.OwnerServer.Business.Tests.Pricing
{
    public class PricingManagerTests
    {
        [Fact]
        public async Task TestPriceCounting()
        {
            var mockUoWFactory = new Mock<IUnitOfWorkFactory<IPricingSettingsUnitOfWork>>();

            var bp = BlueprintFakeData.GetAccommodationBlueprints(1)[0];

            Faker faker = new Faker();
            var refDate = faker.Date.Soon();
            var end = faker.Date.Soon(10, refDate);
            var start = faker.Date.Recent(10, refDate);

            var mockEditor = new Mock<IBlueprintEditor<AccommodationBlueprint>>().Setup((x) => x.GetRequiredSettings(bp)).Returns(()=>new List<TimedSettingSelector>() {new TimedSettingSelector("test1", start,end) });
                       
            
            var mockOptions = Options.Create<PriceManagerOptions>(new PriceManagerOptions() { });
            var pricingManager = new PricingManager(mockUoWFactory.Object, mockOptions);
            
            var cost = await pricingManager.CalculateAccommodationCost(bp);                               


        }
    }
}
