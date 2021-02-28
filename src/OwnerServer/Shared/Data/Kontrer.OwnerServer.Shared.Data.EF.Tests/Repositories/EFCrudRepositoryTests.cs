using FluentAssertions;
using Kontrer.OwnerServer.Shared.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.Shared.Data.EF.Tests.Repositories
{
    public class EFCrudRepositoryTests
    {
        //private readonly PersonEFCrudRepository peopleRepo;
        //private readonly CarEFCrudRepository carRepo;
        private readonly TestUnitOfWork unitOfWork;

        public EFCrudRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase("TestDbContextInMemoryDb").Options;
            var dbContext = new TestDbContext(dbOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
           
            unitOfWork = new TestUnitOfWork(dbContext);
        }

        [Fact]
        public async Task Should_Update_After_Add_Without_Save()
        {
            string newName = "2";

            var model = new PersonModel();
            model.Id = 1;
            model.Name = "1";

            unitOfWork.People.AddAsync(model);            
            model.Name = newName;
            unitOfWork.People.Update(model);

            var newModel = await unitOfWork.People.TryGetAsync(model.Id);
            newModel.Name.Should().Be(newName);
        }

        [Fact]
        public async Task Should_Update_After_Add_With_Save()
        {
            string newName = "2";

            var model = new PersonModel();
            model.Id = 1;
            model.Name = "1";

            unitOfWork.People.AddAsync(model);
            unitOfWork.Commit();
            model.Name = newName;
            unitOfWork.People.Update(model);

            var newModel = await unitOfWork.People.TryGetAsync(model.Id);
            newModel.Name.Should().Be(newName);
        }

      


    }
}
