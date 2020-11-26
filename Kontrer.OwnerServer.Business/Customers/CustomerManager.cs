using Kontrer.OwnerServer.Business.Abstraction.Customers;
using Kontrer.Shared.Repositories.Abstracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Customers
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
