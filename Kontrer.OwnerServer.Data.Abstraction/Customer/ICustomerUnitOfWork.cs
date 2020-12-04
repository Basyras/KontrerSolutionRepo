
using Kontrer.OwnerServer.Data.Abstraction.Customer;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Customers
{
    public interface ICustomerUnitOfWork : IUnitOfWork
    {
        ICustomerRepository Customers { get; }
    }
}
