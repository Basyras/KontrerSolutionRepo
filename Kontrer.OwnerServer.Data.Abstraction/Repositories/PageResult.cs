using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Repositories
{
    public class PageResult<T>
    {
        public PageResult(IEnumerable<T> records, int itemsPerPage, int totalCount, int currentPage, int totalPages)
        {
            Records = records;
            ItemsPerPage = itemsPerPage;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public IEnumerable<T> Records { get; }
        public int ItemsPerPage { get; }
        public int TotalCount { get; }
        public int CurrentPage { get; }
        public int TotalPages { get; }
    }
}
