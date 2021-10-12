using FluentAssertions;
using Kontrer.OwnerServer.IdGeneratorService.Application;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApiTests.IdGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApiTests.IdGenerator
{
    public class CreateNewIdCommandHandlerTests
    {
        private readonly InMemoryIdGeneratorStorage repoMock;
        private readonly CreateNewIdCommandHandler handler;

        public CreateNewIdCommandHandlerTests()
        {
            repoMock = new InMemoryIdGeneratorStorage();
            handler = new CreateNewIdCommandHandler(repoMock);
        }

        [Fact]
        public async Task ReturnsIncrementedIdBeyondCache()
        {
            int currentId;
            int nextId;
            string groupName = "orders";

            for (int i = 0; i < handler.CacheSize + 2; i++)
            {
                currentId = (await handler.Handle(new Domain.CreateNewIdCommand(groupName))).NewId;
                nextId = (await handler.Handle(new Domain.CreateNewIdCommand(groupName))).NewId;
                nextId.Should().Be(currentId + 1, "Generator managers should only return unique values");
                currentId = nextId;
            }
        }

        [Fact]
        public async Task DifferentGroupsShouldHaveRedudantValues()
        {
            string groupName = "orders";
            string groupName2 = "customers";
            var handler = new CreateNewIdCommandHandler(repoMock);
            var group1Id = (await handler.Handle(new Domain.CreateNewIdCommand(groupName))).NewId;
            var group2Id = (await handler.Handle(new Domain.CreateNewIdCommand(groupName2))).NewId;
            group1Id.Should().Be(group2Id, "Two id groups should be independent of each other");
        }
    }
}