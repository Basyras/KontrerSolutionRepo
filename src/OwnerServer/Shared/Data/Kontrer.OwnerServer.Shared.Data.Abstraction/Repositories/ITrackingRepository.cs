using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
   
    public interface ITrackingRepository<TModel, TKey> 
    {
        List<RepositoryAction<TModel, TKey>> Actions { get; }
    }
}