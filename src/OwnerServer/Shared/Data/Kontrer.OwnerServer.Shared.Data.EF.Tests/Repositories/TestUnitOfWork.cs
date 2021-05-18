using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.EF.Tests.Repositories
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private readonly TestDbContext dbContext;

        public TestUnitOfWork(TestDbContext dbContext)
        {
            People = new PersonEFCrudRepository(dbContext);
            Cars = new CarEFCrudRepository(dbContext);
            this.dbContext = dbContext;
        }
        public PersonEFCrudRepository People { get; } 
        public CarEFCrudRepository Cars { get; }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
