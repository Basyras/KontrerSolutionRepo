using Kontrer.Shared.Models;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.CustomerService.Data.EntityFramework
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        //public string  PhoneNumber { get; set; }
        public ContactModel Contact { get; set; }
        public bool IsDeleted { get; set; }

        public List<FinishedAccommodationModel> Accomodations { get; set; } = new List<FinishedAccommodationModel>();

    }
    
}