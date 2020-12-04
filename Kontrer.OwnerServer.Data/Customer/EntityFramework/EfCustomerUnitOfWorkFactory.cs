using Kontrer.OwnerServer.Data.Abstraction.Customers;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Data.EntityFramework;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Customer.EntityFramework
{
    public class EfCustomerUnitOfWorkFactory : IUnitOfWorkFactory<ICustomerUnitOfWork>
    {    

        public EfCustomerUnitOfWorkFactory()
        {           

        }

        public ICustomerUnitOfWork CreateUnitOfWork()
        {
            return new EfCustomerUnitOfWork(new OwnerServerDbContext());
        }
    }
}
