using Kontrer.OwnerServer.Business.Abstraction.Accommodations;
using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kontrer.OwnerServer.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationsController : ControllerBase
    {
        private readonly IAccommodationManager accommodationManager;
        private readonly IPricingManager pricingManager;
        private readonly ILogger<AccommodationsController> logger;
        private readonly IAccommodationUnitOfWork unitOfWork;

        public AccommodationsController(IAccommodationManager accommodationManager, IPricingManager pricingManager, ILogger<AccommodationsController> logger)
        {
            this.accommodationManager = accommodationManager;
            this.pricingManager = pricingManager;
            this.logger = logger;
            unitOfWork = this.accommodationManager.CreateUnitOfWork();
        }

        // GET: api/<AccomodationsController>
        [HttpGet]
        public async Task<ActionResult<Dictionary<int, AccommodationModel>>> Get()
        {
            try
            {
                Dictionary<int, AccommodationModel> records = await unitOfWork.Accommodations.GetAllAsync();
                return this.Ok(records);
            }
            catch (Exception ex)
            {
                string message = $"Error while quereing all accommodation";
                logger.LogError(message, ex);
                return Problem(message);
            }
        }

        // GET api/<AccomodationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccommodationModel>> Get(int id)
        {
            var record = await unitOfWork.Accommodations.GetAsync(id);
            return record == null ? this.NotFound(null) : this.Ok(record);

        }

        // POST api/<AccomodationsController>
        [HttpPost]
        //public async Task<IActionResult> Post(int customerId, [FromBody] AccommodationBlueprint blueprint, AccommodationCost cost = null)
        public async Task<IActionResult> Post(int customerId, [FromBody] AccommodationBlueprint blueprint)
        {
            //cost = cost ?? await pricingManager.CalculateAccommodationCost(blueprint);
            var cost = await pricingManager.CalculateAccommodationCostAsync(blueprint);
            unitOfWork.Accommodations.Create(customerId, cost, blueprint);
            try
            {
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error while creating accommodation, customerId {customerId} and accommodation start: {blueprint.Start}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }

        // PUT api/<AccomodationsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AccommodationModel value)
        {
            unitOfWork.Accommodations.Edit(value);
            try
            {
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error with edit of accommodation, customerId {value.Customer.CustomerId} and accommodation start: {value.Blueprint.Start}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }

        // DELETE api/<AccomodationsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id, bool canceledByCustomer, string notes = null)
        {
            unitOfWork.Accommodations.Cancel(id, canceledByCustomer, notes);
            try
            {
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error when Delete, accommodationId {id}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }
    }
}
