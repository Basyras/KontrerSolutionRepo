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
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

    }
}
