
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Business.Abstraction.Customers
{
    public interface ICustomerManager 
    {
        ICustomerUnitOfWork CreateUnitOfWork();

    }
}
