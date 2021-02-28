using Kontrer.OwnerServer.CustomerService.Business.Abstraction;
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

        private readonly ICustomerService customerService;
        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
            
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public Task<Dictionary<int, CustomerModel>> GetAllCustomers()
        {
            return customerService.GetAllCustomersAsync();
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public Task<CustomerModel> GetCustomer(int customerId)
        {
            return customerService.GetCustomerAsync(customerId);
        }

        // POST api/<CustomersController>
        [HttpPost]
        public Task CreateCustomer([FromBody] CustomerModel customer)
        {
            return customerService.CreateCustomerAsync(customer);
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public Task UpdateCustomer(int id, [FromBody] CustomerModel customer)
        {
            return customerService.ChangeCustomerDetailsAsync(customer);
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public Task DeleteCustomer(int customerId)
        {
            return customerService.DeleteCustomerAsync(customerId);
        }
    }
}
