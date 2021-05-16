using Kontrer.OwnerServer.PricingService.Application.Processing;
using Kontrer.OwnerServer.PricingService.Application.Processing.Pricers;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.PricingService.Application.Tests.Pricing.Pricers
{
    public class ItemTaxPricerTests
    {
        [Theory]
        [InlineData(1D,0.2F)]
        [InlineData(6D,2F)]
        [InlineData(100D,0.05F)]
        [InlineData(1528D, 0.9F)]
        public void Should_Calculate_Tax(decimal baseSubTotal, float itemTax)
        {

            var itemBb = new ItemBlueprint(new Cash(Currencies.CZK, 1), 1, itemTax,"name");
            var accoBp = new AccommodationBlueprint(Currencies.CZK, DateTime.Now, DateTime.Now.AddDays(1), new CustomerModel(), null, new List<ItemBlueprint>() { itemBb }); 
            var rawCost = new RawItemCost(itemBb);
            rawCost.ManipulateCost("test", baseSubTotal);
            RawAccommodationCost rawAccoCost = new(Currencies.CZK, new List<RawItemCost>() { rawCost }, new List<RawRoomCost>());


            var newSubTotal = baseSubTotal  + (baseSubTotal * (decimal)itemTax);
            var pricer = new AccommodationItemTaxPricer();
            
            pricer.CalculateContractCost(accoBp, rawAccoCost, null);
            Assert.Equal(newSubTotal, rawAccoCost.RawAccommodationItems[0].SubTotal);
        }
    }
}
