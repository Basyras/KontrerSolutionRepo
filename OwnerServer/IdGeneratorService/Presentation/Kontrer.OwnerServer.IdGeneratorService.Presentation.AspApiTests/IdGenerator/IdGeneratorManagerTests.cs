using FluentAssertions;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Abstraction.Data;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApiTests.IdGenerator.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApiTests.IdGenerator
{
    public class IdGeneratorManagerTests
    {
        [Fact]
        public void ManagerReturnsUniqueIdBeyondCache()
        {
            var storageMoq = new InMemoryIdGeneratorStorage();
            var manager = new IdGeneratorManager(storageMoq);

            int currentId = 0;
            int nextId = 0;

            for (int i = 0; i < manager.CacheSize + 2; i++)
            {
                currentId = manager.CreateNewId(IdGeneratorManager.OrdersGroupName);
                nextId = manager.CreateNewId(IdGeneratorManager.OrdersGroupName);
                nextId.Should().Be(currentId + 1, "Generator managers must return only unique values");
                currentId = nextId;
            }


        }

        [Fact]
        public void DifferentGroupsShouldHaveRedudantValues()
        {
            var storageMoq = new InMemoryIdGeneratorStorage();

            var manager = new IdGeneratorManager(storageMoq);
            var group1Id = manager.CreateNewId(IdGeneratorManager.OrdersGroupName);
            var group2Id = manager.CreateNewId(IdGeneratorManager.CustomersGroupName);
            group1Id.Should().Be(group2Id, "Two id groups should be independent of each other");
        }

        [Fact]
        public void UnknownGroupNameShouldFail()
        {
            var storageMoq = new InMemoryIdGeneratorStorage();
            var manager = new IdGeneratorManager(storageMoq);

            Action action = () => { manager.CreateNewId("RandomGroupName"); };
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void KnownGroupNameShouldPass()
        {
            var storageMoq = new InMemoryIdGeneratorStorage();
            var manager = new IdGeneratorManager(storageMoq);
            _ = manager.CreateNewId(IdGeneratorManager.OrdersGroupName);

        }
    }
}
