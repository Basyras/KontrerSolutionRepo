using Kontrer.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kontrer.OwnerServer.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {


        // GET: api/<CustomersController>
        [HttpGet]        
        public IEnumerable<CustomerModel> Get()
        {
            return new CustomerModel[0];
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public CustomerModel Get(int id)
        {
            return new CustomerModel();
        }

        // POST api/<CustomersController>
        [HttpPost]
        public void Post([FromBody] CustomerModel value)
        {
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CustomerModel value)
        {
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
