using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.Tests.EntityFramework
{
    public class EfPricingSettingsRepositoryTests
    {
        private readonly EfPricingSettingsRepository repository;

        public EfPricingSettingsRepositoryTests()
        {
            //var dbOptions = new DbContextOptionsBuilder<PricingServiceDbContext>().UseInMemoryDatabase("TestDbContextInMemoryDb").Options;
                                                                                                 
                                                                                                 //"Data Source=(localdb)\\MSSQLLocalDB;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;Database=PricingServiceDB;Trusted_Connection=True"
            var dbOptions = new DbContextOptionsBuilder<PricingServiceDbContext>().UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;Database=PricingServiceTest").Options;
            //var dbOptions = new DbContextOptionsBuilder<PricingServiceDbContext>().UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;Database=PricingServiceDB;Trusted_Connection=True").Options;
            var dbContext = new PricingServiceDbContext(dbOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            repository = new EfPricingSettingsRepository(dbContext);
            
        }

        
        [Fact]
        public async Task GetScopedSettingsAsync_Should_Have_Unique_SettingIds()
        {
            repository.CreateNewSetting<int>("setting1");
            repository.CreateNewSetting<int>("setting2");
            repository.CreateNewSetting<int>("setting3");
            repository.CreateNewSetting<int>("setting4");

            repository.CreateNewTimeScope("scope1", DateTime.Now, DateTime.Now.AddDays(1));
            repository.CreateNewTimeScope("scope2", DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1));
            repository.CreateNewTimeScope("scope3", DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(2));
            repository.CreateNewTimeScope("scope4", DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3));
            repository.Save();


            var allScopes = await repository.GetTimeScopesAsync();
            repository.AddScopedSetting<int>("setting1", allScopes.First(x => x.Name == "scope1").Id, 11);
            repository.AddScopedSetting<int>("setting1", allScopes.First(x => x.Name == "scope2").Id, 12);
            repository.AddScopedSetting<int>("setting1", allScopes.First(x => x.Name == "scope3").Id, 13);
            repository.AddScopedSetting<int>("setting1", allScopes.First(x => x.Name == "scope4").Id, 14);

            repository.AddScopedSetting<int>("setting2", allScopes.First(x => x.Name == "scope1").Id, 21);
            repository.AddScopedSetting<int>("setting2", allScopes.First(x => x.Name == "scope2").Id, 22);
            repository.AddScopedSetting<int>("setting2", allScopes.First(x => x.Name == "scope3").Id, 23);
            repository.AddScopedSetting<int>("setting2", allScopes.First(x => x.Name == "scope4").Id, 24);

            repository.AddScopedSetting<int>("setting3", allScopes.First(x => x.Name == "scope3").Id, 3);
            repository.AddScopedSetting<int>("setting4", allScopes.First(x => x.Name == "scope4").Id, 4);
            repository.Save();

            var allSettings = await repository.GetAllScopedSettingsAsync();

            var settings = await repository.GetScopedSettingsAsync(new List<ScopedSettingRequest> 
            { 
                new ScopedSettingRequest(new Application.Processing.SettingRequest("setting2"),allScopes.First(x=>x.Name == "scope2").Id), 
            });
        }

        [Fact]
        public async Task GetTimeScopes_When_Empty_Should_Return_Empty_Collection()
        {
            var scopes = await repository.GetTimeScopesAsync();
            Assert.NotNull(scopes);
        }
    }
}
