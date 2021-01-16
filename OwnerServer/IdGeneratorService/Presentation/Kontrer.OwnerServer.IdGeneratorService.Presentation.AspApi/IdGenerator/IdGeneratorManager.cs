using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator
{
    public class IdGeneratorManager
    {

        public const string OrdersGroupName = "orders";
        public const string CustomersGroupName = "customers";


        private readonly IIdGeneratorStorage idGeneratorStorage;
        /// <summary>
        /// Key is group name
        /// </summary>
        private readonly Dictionary<string,Queue<int>> cachedIds;
        public readonly int CacheSize = 50;
        

        public IdGeneratorManager(IIdGeneratorStorage idGeneratorStorage)
        {
            this.idGeneratorStorage = idGeneratorStorage;
            cachedIds = new Dictionary<string, Queue<int>>();
            cachedIds.Add(OrdersGroupName, new Queue<int>(CacheSize));
            cachedIds.Add(CustomersGroupName, new Queue<int>(CacheSize));
        }

        public int CreateNewId(string groupName)
        {
            var groupExits = cachedIds.TryGetValue(groupName, out Queue<int> queue);
            if(groupExits is false)
            {
                throw new ArgumentException(nameof(groupName));
            }

            if (queue.Count == 0)
            {
                var ids = GenerateIds(groupName).GetAwaiter().GetResult();
                //for (int i = 0; i < CacheSize; i++)
                foreach(var id in ids)
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
            return Enumerable.Range(lastUsedId, CacheSize);

        }
    }
}
