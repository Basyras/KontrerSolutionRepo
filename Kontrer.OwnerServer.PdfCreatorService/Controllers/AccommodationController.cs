using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kontrer.OwnerServer.PdfCreatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationController : ControllerBase
    {
        // GET: api/<AccommodationController>
        [HttpGet]
        public MemoryStream Get(AccommodationCost cost)
        {
            throw new NotImplementedException();
        }      

      
    }
}
