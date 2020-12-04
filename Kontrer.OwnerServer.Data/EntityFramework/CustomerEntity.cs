using Kontrer.Shared.Models;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.Data.EntityFramework
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }

        public List<AccommodationModel> Accomodations { get; set; }

    }
    
}