using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customers;
using Kontrer.OwnerServer.CustomerService.Data.EntityFramework;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Customer.EntityFramework
{
    public class EfCustomerUnitOfWorkFactory : IUnitOfWorkFactory<ICustomerUnitOfWork>
    {    

        public EfCustomerUnitOfWorkFactory()
        {           

        }

        public ICustomerUnitOfWork CreateUnitOfWork()
        {
            return new EfCustomerUnitOfWork(new CustomerServiceDbContext());
        }
    }
}
