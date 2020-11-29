using Kontrer.OwnerServer.Business.Pricing;
using Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares;
using Kontrer.Shared.Models.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.Business.Tests.Pricing.PricingMiddlewares
{
    public class ItemTaxMiddlewareTests
    {
        [Theory]
        [InlineData(1D,0.2F)]
        [InlineData(6D,2F)]
        [InlineData(100D,0.05F)]
        [InlineData(1528D, 0.9F)]
        public void BasicCalc(decimal baseSubTotal, float itemTax)
        {


            var itemBb = new ItemBlueprint(new Cash(Currencies.CZK, 1), 1, itemTax);            
            var accoBp = new AccommodationBlueprint(Currencies.CZK, DateTime.Now, DateTime.Now.AddDays(1), null, new List<ItemBlueprint>() { itemBb });
            var rawCost = new RawItemCost(itemBb);
            rawCost.ManipulateCost("test", baseSubTotal);
            RawAccommodationCost rawAccoCost = new(Currencies.CZK, new List<RawItemCost>() { rawCost }, new List<RawRoomCost>());


            var newSubTotal = baseSubTotal  + (baseSubTotal * (decimal)itemTax);
            var pricer = new ItemTaxMiddleware();
            
            pricer.CalculateContractCost(accoBp, rawAccoCost, null);
            Assert.Equal(newSubTotal, rawAccoCost.RawAccommodationItems[0].SubTotal);
        }
    }
}
