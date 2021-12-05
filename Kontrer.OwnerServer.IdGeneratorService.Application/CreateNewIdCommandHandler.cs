using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.IdGeneratorService.Application.Interfaces;
using Kontrer.OwnerServer.IdGeneratorService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Application
{
    //Need to move logic to different class since handler is created for every request
    public class CreateNewIdCommandHandler : ICommandHandler<CreateNewIdCommand, GetNewIdResponse>
    {
        private readonly IIdGeneratorRepository repository;
        private readonly Dictionary<string, Queue<int>> cachedIds = new Dictionary<string, Queue<int>>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public int CacheSize { get; set; } = 50;

        public CreateNewIdCommandHandler(IIdGeneratorRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetNewIdResponse> Handle(CreateNewIdCommand request, CancellationToken cancellationToken = default)
        {
            var newId = await GetNewId(request.groupName);
            return new GetNewIdResponse(newId);
        }

        private async Task<int> GetNewId(string groupName)
        {
            await semaphore.WaitAsync();
            var groupExits = cachedIds.TryGetValue(groupName, out Queue<int> queue);
            if (groupExits is false)
            {
                queue = new Queue<int>(CacheSize);
                cachedIds.Add(groupName, queue);
            }

            if (queue.Count == 0)
            {
                var ids = await GenerateIds(groupName);
                foreach (var id in ids)
                {
                    queue.Enqueue(id);
                }
            }

            int newId = queue.Dequeue();
            semaphore.Release();
            return newId;
        }

        private async Task<IEnumerable<int>> GenerateIds(string groupName)
        {
            int lastUsedId = await repository.GetLastUsedId(groupName);
            await repository.SetLastUsedId(groupName, lastUsedId + CacheSize);
            return Enumerable.Range(lastUsedId + 1, CacheSize);
        }
    }
}