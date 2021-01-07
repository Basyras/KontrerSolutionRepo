using Kontrer.OwnerServer.CustomerService.Business.Abstraction.Accommodations;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
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

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationsController : ControllerBase
    {
        private readonly IAccommodationManager accommodationManager;
        
        private readonly ILogger<AccommodationsController> logger;
        private readonly IAccommodationUnitOfWork unitOfWork;

        public AccommodationsController(IAccommodationManager accommodationManager,  ILogger<AccommodationsController> logger)
        {
            this.accommodationManager = accommodationManager;
            
            this.logger = logger;
            unitOfWork = this.accommodationManager.CreateUnitOfWork();
        }

        // GET: api/<AccomodationsController>
        [HttpGet]
        public async Task<ActionResult<Dictionary<int, FinishedAccommodationModel>>> Get()
        {
            try
            {
                Dictionary<int, FinishedAccommodationModel> records = await unitOfWork.Accommodations.GetAllAsync();
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
        public async Task<ActionResult<FinishedAccommodationModel>> Get(int id)
        {
            var record = await unitOfWork.Accommodations.GetAsync(id);
            return record == null ? this.NotFound(null) : this.Ok(record);

        }

        // POST api/<AccomodationsController>
        [HttpPost]        
        public async Task<IActionResult> Post(int customerId, [FromBody] AccommodationBlueprint blueprint)
        {           
            
            unitOfWork.Accommodations.Add(customerId, null, blueprint);
            try
            {
                await unitOfWork.CommitAsync();
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
        public async Task<ActionResult> Put(int id, [FromBody] FinishedAccommodationModel value)
        {
            unitOfWork.Accommodations.Edit(value);
            try
            {
                await unitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error with edit of accommodation, customerId {value.CustomerId} and accommodation start: {value.Order.Blueprint.Start}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }

        // DELETE api/<AccomodationsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            unitOfWork.Accommodations.Remove(id);
            try
            {
                await unitOfWork.CommitAsync();
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
