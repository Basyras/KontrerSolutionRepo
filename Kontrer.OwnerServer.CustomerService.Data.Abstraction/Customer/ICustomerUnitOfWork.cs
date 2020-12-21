
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customer;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customers
{
    public interface ICustomerUnitOfWork : IUnitOfWork
    {
        ICustomerRepository Customers { get; }
    }
}
