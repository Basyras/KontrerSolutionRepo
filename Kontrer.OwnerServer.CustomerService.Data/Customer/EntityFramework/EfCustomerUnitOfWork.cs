using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customer;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customers;
using Kontrer.OwnerServer.CustomerService.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Customer.EntityFramework
{
    public class EfCustomerUnitOfWork : ICustomerUnitOfWork
    {
        private readonly CustomerServiceDbContext dbContext;

        public EfCustomerUnitOfWork(CustomerServiceDbContext dbContext)
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
