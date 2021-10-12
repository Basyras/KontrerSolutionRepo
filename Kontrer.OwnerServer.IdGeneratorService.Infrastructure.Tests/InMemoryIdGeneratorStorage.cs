using Kontrer.OwnerServer.IdGeneratorService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApiTests.IdGenerator.Data
{
    public class InMemoryIdGeneratorStorage : IIdGeneratorRepository
    {
        private Dictionary<string, int> storage = new Dictionary<string, int>();

        public InMemoryIdGeneratorStorage(int miliSecondsDelay = 0)
        {
            this.MiliSecondsDelay = miliSecondsDelay;
        }

        public int MiliSecondsDelay { get; set; }

        public async Task<int> GetLastUsedId(string groupName)
        {
            TryCreate(groupName);
            var id = storage[groupName];
            await Task.Delay(MiliSecondsDelay);
            return id;
        }

        public async Task SetLastUsedId(string groupName, int lastUsedId)
        {
            TryCreate(groupName);
            storage[groupName] = lastUsedId;
            await Task.Delay(MiliSecondsDelay);
        }

        private void TryCreate(string groupName)
        {
            var groupExists = storage.TryGetValue(groupName, out _);
            if (groupExists is false)
            {
                storage.Add(groupName, default);
            }
        }
    }
}