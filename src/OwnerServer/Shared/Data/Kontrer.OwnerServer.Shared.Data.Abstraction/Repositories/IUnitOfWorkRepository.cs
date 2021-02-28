using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    /// <summary>
    /// Repositories used inside unit of work should not have save/commit/complete public implementation
    /// </summary>
    public interface IUnitOfWorkRepository<TModel,TKey> : ITrackingRepository<TModel, TKey>
    {

    }
}
