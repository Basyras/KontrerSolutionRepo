using Kontrer.OwnerServer.PricingService.Business.Pricing;
using Kontrer.OwnerServer.PricingService.Business.Pricing.PricingMiddlewares;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Kontrer.Shared.Tests.FakeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.PricingService.Business.Tests.Pricing.PricingMiddlewares
{
    public class BaseCostMiddlewareTests
    {
        [Theory]
        [InlineData(1,1)]
        [InlineData(6,1)]
        [InlineData(100,50)]
        [InlineData(9,68)]
        public void CanCalculate(int count,int costPerOne)
        {
            var itemBb = new ItemBlueprint(new Cash(Currencies.CZK, costPerOne), count, 0);
            var accoBp = new AccommodationBlueprint(Currencies.CZK, DateTime.Now, DateTime.Now.AddDays(1), new CustomerModel(),null, new List<ItemBlueprint>() { itemBb, itemBb });          
            RawAccommodationCost rawAccoCost = new(Currencies.CZK, new List<RawItemCost>() { new RawItemCost(itemBb), new RawItemCost(itemBb) }, new List<RawRoomCost>());


            var newSubTotal = 2* itemBb.Count * itemBb.CostPerOne.Amount;
            var pricer = new AccommodationBaseCostPricer();
            pricer.CalculateContractCost(accoBp, rawAccoCost, null);
            Assert.Equal(newSubTotal, rawAccoCost.RawAccommodationItems.Sum(x=>x.SubTotal));


        }

        [Fact]
        public void Test()
        {
            ItemBlueprint bp = new ItemBlueprint();
            bp.CanParentApplyDiscount = true;
            ChangeBp(bp);
            Assert.False(bp.CanParentApplyDiscount);
        }

        private void ChangeBp (ItemBlueprint bp)
        {
            bp.CanParentApplyDiscount = false;
        }
    }
}
