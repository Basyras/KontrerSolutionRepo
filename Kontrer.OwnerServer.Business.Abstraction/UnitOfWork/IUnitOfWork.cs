using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {


        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default);


    }
}
