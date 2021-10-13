using FluentAssertions;
using Kontrer.OwnerServer.IdGeneratorService.Application;
using Kontrer.OwnerServer.IdGeneratorService.Domain;
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
        private static readonly string groupNameOrders = "orders";
        private static readonly string groupNameCustomers = "customers";

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

            for (int i = 0; i < handler.CacheSize + 2; i++)
            {
                currentId = (await handler.Handle(new Domain.CreateNewIdCommand(groupNameOrders))).NewId;
                nextId = (await handler.Handle(new Domain.CreateNewIdCommand(groupNameOrders))).NewId;
                nextId.Should().Be(currentId + 1, "Generator managers should only return unique values");
                currentId = nextId;
            }
        }

        [Fact]
        public async Task DifferentGroupsShouldHaveRedudantValues()
        {
            var group1Id = (await handler.Handle(new Domain.CreateNewIdCommand(groupNameOrders))).NewId;
            var group2Id = (await handler.Handle(new Domain.CreateNewIdCommand(groupNameCustomers))).NewId;
            group1Id.Should().Be(group2Id, "Two id groups should be independent of each other");
        }

        [Fact]
        public async Task First_Id_Should_Be_One()
        {
            var firstId = (await handler.Handle(new Domain.CreateNewIdCommand(groupNameOrders))).NewId;
            firstId.Should().Be(1);
        }

        [Fact]
        public async Task Concurrency_Should_Be_Queued()
        {
            int taskCount = 3;
            var command = new CreateNewIdCommand(groupNameOrders);
            handler.CacheSize = 1;
            repoMock.MiliSecondsDelay = 50;
            var tasks = Enumerable.Range(0, taskCount)
               .Select<int, Task<GetNewIdResponse>>((x) =>
               {
                   return handler.Handle(command);
               });

            var responses = await Task.WhenAll(tasks.AsParallel().Select(async (task) =>
            {
                await Task.Delay(600);
                return await task;
            }));

            for (int i = 0; i < taskCount; i++)
            {
                responses.Should().Contain(x => x.NewId == i + 1);
            }
        }
    }
}