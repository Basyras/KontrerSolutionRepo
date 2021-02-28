using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    public interface IPageRepository<TModel>
    {
        Task<PageResult<TModel>> GetPageAsync(int page, int itemsPerPage);
    }
}
