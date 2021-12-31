using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
    public class CustomerEntity
    {
        public CustomerEntity(int id, string firstName, string secondName, string email)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            Email = email;
        }

        public CustomerEntity()
        {

        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
    }
}