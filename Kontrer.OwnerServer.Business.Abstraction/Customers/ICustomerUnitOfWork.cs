
using Kontrer.OwnerServer.Business.Abstraction.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Customers
{
    public interface ICustomerUnitOfWork : IUnitOfWork
    {
        ICustomerRepository Customers { get; }
    }
}
