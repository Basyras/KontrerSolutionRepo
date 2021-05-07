using Kontrer.OwnerServer.CustomerService.Business.Abstraction.Accommodations;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
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
        private readonly IMessageBusManager messageBusManager;
        private readonly ILogger<AccommodationsController> logger;
        

        public AccommodationsController(IAccommodationManager accommodationManager, IMessageBusManager messageBusManager, ILogger<AccommodationsController> logger)
        {
            this.accommodationManager = accommodationManager;
            this.messageBusManager = messageBusManager;
            this.logger = logger;
            unitOfWork = this.accommodationManager.CreateUnitOfWork();
        }

        // GET: api/<AccomodationsController>
        [HttpGet]
        public async Task<ActionResult<Dictionary<int, FinishedAccommodationModel>>> GetAccommodations()
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
        public async Task<ActionResult<FinishedAccommodationModel>> GetAccommodation(int id)
        {
            var record = await unitOfWork.Accommodations.GetAsync(id);
            return record == null ? this.NotFound(null) : this.Ok(record);

        }

        // POST api/<AccomodationsController>
        [HttpPost]
        public async Task<IActionResult> AddAccommodation(int customerId, int orderId, string privateOwnersNotes, [FromBody] AccommodationBlueprint blueprint, [FromBody] AccommodationCost cost)
        {
            var accommodationId = await messageBusManager.RequestAsync<CreateAccommodationIdRequest, int>();

            unitOfWork.Accommodations.AddAsync(new FinishedAccommodationModel(accommodationId, customerId, orderId, cost, privateOwnersNotes));

            try
            {
                await unitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error while {nameof(AddAccommodation)}, customerId: {customerId}, accommodation start: {blueprint.Start}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }

        // PUT api/<AccomodationsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccommodation([FromBody] FinishedAccommodationModel newAccommodation)
        {
            unitOfWork.Accommodations.Edit(newAccommodation);
            try
            {
                await unitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error with {nameof(UpdateAccommodation)}, customerId {newAccommodation.CustomerId} and accommodation start: {newAccommodation.Blueprint.Start}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }

        // DELETE api/<AccomodationsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccommodation(int id)
        {
            unitOfWork.Accommodations.RemoveAsync(id);
            try
            {
                await unitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = $"Error with {nameof(DeleteAccommodation)}, accommodationId {id}";
                logger.LogError(ex, message);
                return Problem(message);
            }

        }
    }
}
