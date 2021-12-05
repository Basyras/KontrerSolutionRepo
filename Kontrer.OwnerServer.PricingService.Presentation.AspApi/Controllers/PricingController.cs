using Basyc.Shared.Models.Pricing.Costs;
using Kontrer.OwnerServer.PricingService.Application;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly PricingManager _pricingManager;

        public PricingController(PricingManager pricingManager)
        {
            _pricingManager = pricingManager;
        }

        [HttpPost]
        public async Task<ActionResult<AccommodationCost>> CalculateAccommodationCost(AccommodationBlueprint blueprint)
        {          
            try
            {
                AccommodationCost cost = await _pricingManager.CalculateAccommodationCostAsync(blueprint);
                return Ok(cost);
            }
            catch(NoSuitableTimeScopeFoundException ex)
            {
                return Problem(ex.Message);
            }
           
        }


    }
}
