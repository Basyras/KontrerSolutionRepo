using Kontrer.Shared.Models.Costs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.Shared.Models
{
    public class Customer
    {
        public Customer()
        {

          
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

        public List<AccommodationModel> Accomodations { get; set; }



    }
}
