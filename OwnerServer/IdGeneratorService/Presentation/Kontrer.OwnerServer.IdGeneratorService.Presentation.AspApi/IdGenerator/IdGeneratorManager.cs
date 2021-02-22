using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator
{
    public class IdGeneratorManager : IIdGeneratorManager
    {
        private readonly IIdGeneratorStorage idGeneratorStorage;
        /// <summary>
        /// Key is group name
        /// </summary>
        private readonly Dictionary<string, Queue<int>> cachedIds;
        public int CacheSize { get; set; } = 50;      


        public IdGeneratorManager(IIdGeneratorStorage idGeneratorStorage)
        {
            this.idGeneratorStorage = idGeneratorStorage;
            cachedIds = new Dictionary<string, Queue<int>>();
            cachedIds.Add(IIdGeneratorManager.OrdersGroupName, new Queue<int>(CacheSize));
            cachedIds.Add(IIdGeneratorManager.CustomersGroupName, new Queue<int>(CacheSize));
        }


        public int CreateNewId(string groupName)
        {
            var groupExits = cachedIds.TryGetValue(groupName, out Queue<int> queue);
            if (groupExits is false)
            {
                throw new ArgumentException(nameof(groupName));
            }

            if (queue.Count == 0)
            {
                var ids = GenerateIds(groupName).GetAwaiter().GetResult();                
                foreach (var id in ids)
                {
                    queue.Enqueue(id);
                }
            }

            int newId = queue.Dequeue();
            return newId;
        }

        private async Task<IEnumerable<int>> GenerateIds(string groupName)
        {

            int lastUsedId = await idGeneratorStorage.GetLastUsedId(groupName);
            await idGeneratorStorage.SetLastUsedId(groupName, lastUsedId + CacheSize);
            return Enumerable.Range(lastUsedId+1, CacheSize);

        }
    }
}
