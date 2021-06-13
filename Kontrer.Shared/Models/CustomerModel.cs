using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.Shared.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ContactModel Contact { get; set; }
        public AddressModel Address { get; set; }
    }
}