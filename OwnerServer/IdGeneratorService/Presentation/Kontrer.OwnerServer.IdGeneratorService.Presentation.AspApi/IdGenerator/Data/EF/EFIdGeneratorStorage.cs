using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Abstraction.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Data.EF
{
    public class EFIdGeneratorStorage : IIdGeneratorStorage
    {
        private readonly DbContext dbContext;

        public EFIdGeneratorStorage(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> GetLastUsedId(string groupName)
        {
            var record = await dbContext.Set<LastUsedIdEntity>().FindAsync(groupName);
            if (record is null)
            {
                var newEntity = new LastUsedIdEntity() { GroupName = groupName, LastUsedId = 1 };
                dbContext.Set<LastUsedIdEntity>().Add(newEntity);
                await dbContext.SaveChangesAsync();
                return newEntity.LastUsedId;
            }
            else
            {
                return record.LastUsedId;
            }
        }

        public Task SetLastUsedId(string groupName, int lastUsedId)
        {         

            var oldEntity = dbContext.Set<LastUsedIdEntity>().Find(groupName);

            if (oldEntity is null)
            {
                var newEntity = new LastUsedIdEntity()
                {
                    GroupName = groupName,
                    LastUsedId = lastUsedId
                };
                dbContext.Set<LastUsedIdEntity>().Add(newEntity);
            }
            else
            {
                oldEntity.LastUsedId = lastUsedId;
                dbContext.Set<LastUsedIdEntity>().Update(oldEntity);
            }

            return dbContext.SaveChangesAsync();

        }
    }
}
