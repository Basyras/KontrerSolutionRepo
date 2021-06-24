using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories
{
    /// <summary>
    /// All repositories must inherit <see cref="IUnitOfWorkRepository{TModel, TKey}"/>
    /// <br/> This class should containg all repositories as readonly properties
    /// </summary>
    public interface IUnitOfWork : IBulkRepository, IDisposable
    {
    }
}