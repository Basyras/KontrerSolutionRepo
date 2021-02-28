using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    public interface IUnitOfWorkFactory<T> where T : class, IUnitOfWork
    {
        T CreateUnitOfWork();
    }
}
