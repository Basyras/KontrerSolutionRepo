using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Application.Interfaces
{
    public interface IIdGeneratorRepository
    {
        /// <summary>
        /// Group is creted if not exists
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<int> GetLastUsedId(string groupName);

        /// <summary>
        /// Group is creted if not exists
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="lastUsedId"></param>
        /// <returns></returns>
        Task SetLastUsedId(string groupName, int lastUsedId);
    }
}