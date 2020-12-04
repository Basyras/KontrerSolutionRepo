using Kontrer.OwnerServer.Data.Abstraction.Customer;
using Kontrer.OwnerServer.Data.Abstraction.Customers;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.OwnerServer.Data.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Customer.EntityFramework
{
    public class EfCustomerUnitOfWork : ICustomerUnitOfWork
    {
        private readonly OwnerServerDbContext dbContext;

        public EfCustomerUnitOfWork(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
            Customers = new EfCustomerRepository(dbContext);
        }

        public ICustomerRepository Customers { get; }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            dbContext.Dispose();

        }
    }
}
