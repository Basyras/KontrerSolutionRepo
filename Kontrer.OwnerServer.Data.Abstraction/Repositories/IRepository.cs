using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Repositories
{
    public interface IRepository : IDisposable
    {
        void Save();
        Task SaveAsync(CancellationToken cancellationToken = default);        
    }
}