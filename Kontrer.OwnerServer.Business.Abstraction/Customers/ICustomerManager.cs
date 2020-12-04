
using Kontrer.OwnerServer.Data.Abstraction.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Customers
{
    public interface ICustomerManager 
    {
        ICustomerUnitOfWork CreateUnitOfWork();
    }
}
