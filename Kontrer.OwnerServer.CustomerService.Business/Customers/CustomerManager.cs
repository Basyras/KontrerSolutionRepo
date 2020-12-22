using Kontrer.OwnerServer.CustomerService.Business.Abstraction.Customers;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customers;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Business.Customers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IUnitOfWorkFactory<ICustomerUnitOfWork> unitOfWorkFactory;

        public CustomerManager(IUnitOfWorkFactory<ICustomerUnitOfWork> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ICustomerUnitOfWork CreateUnitOfWork()
        {
            throw new NotImplementedException();
        }

      
    }
}
