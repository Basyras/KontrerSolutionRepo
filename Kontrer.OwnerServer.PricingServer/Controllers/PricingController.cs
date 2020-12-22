using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly IPricingManager pricingManager;

        public PricingController(IPricingManager pricingManager)
        {
            this.pricingManager = pricingManager;
        }

        [HttpPost]
        public async Task<AccommodationCost> CalculateAccommodationCost(AccommodationBlueprint blueprint)
        {
            AccommodationCost cost = await pricingManager.CalculateAccommodationCostAsync(blueprint);
            return cost;
        }
    }
}
