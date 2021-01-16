using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Abstraction.Data
{
    public interface IIdGeneratorStorage
    {
        Task<int> GetLastUsedId(string groupName);
        Task SetLastUsedId(string groupName, int lastUsedId);
    }
}
