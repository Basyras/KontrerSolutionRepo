using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    public interface IPageniationRepository<TModel> : IRepository
    {
        PageResult<TModel> GetPage(int page, int itemsPerPage);
    }
}
