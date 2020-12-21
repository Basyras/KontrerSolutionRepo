using Kontrer.OwnerServer.CustomerService.Business.Abstraction.Customers;
using Kontrer.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomerManager customerManager;
        public CustomersController(ICustomerManager customerManager = null)
        {
            this.customerManager = customerManager;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public async Task<Dictionary<int, CustomerModel>> Get()
        {
            using var work = customerManager.CreateUnitOfWork();
            Dictionary<int, CustomerModel> customers = await work.Customers.GetAllAsync();
            return customers;
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public async Task<CustomerModel> Get(int id)
        {
            using var work = customerManager.CreateUnitOfWork();
            var customer = await work.Customers.GetAsync(id);
            return customer;
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
