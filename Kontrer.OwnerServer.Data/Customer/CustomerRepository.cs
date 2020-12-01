using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Customer
{
    class CustomerRepository
    {
        private readonly OwnerServerDbContext dbContext;

        public CustomerRepository(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public static CustomerModel ToModel(CustomerEntity entity)
        {
            CustomerModel model = new CustomerModel();
            model.Accomodations = entity.Accomodations;
            model.CustomerId = entity.CustomerId;
            model.Email = entity.Email;
            model.FirstName = entity.FirstName;
            model.PhoneNumber = entity.PhoneNumber;
            model.SecondName = entity.SecondName;
            return model;
        }
        public static CustomerEntity ToEntity(CustomerModel model)
        {
            CustomerEntity entity = new CustomerEntity();
            entity.Accomodations = model.Accomodations;
            entity.CustomerId = model.CustomerId;
            entity.Email = model.Email;
            entity.FirstName = model.FirstName;
            entity.PhoneNumber = model.PhoneNumber;
            entity.SecondName = model.SecondName;
            return entity;
        }
    }
}
