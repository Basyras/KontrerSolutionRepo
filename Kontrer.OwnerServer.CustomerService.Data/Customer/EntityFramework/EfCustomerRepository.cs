using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customer;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.CustomerService.Data.EntityFramework;
using Kontrer.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Customer.EntityFramework
{
    public class EfCustomerRepository : ICustomerRepository
    {
        private readonly CustomerServiceDbContext dbContext;

        public EfCustomerRepository(CustomerServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        internal static CustomerModel ToModel(CustomerEntity entity)
        {
            CustomerModel model = new CustomerModel();
            model.CustomerId = entity.CustomerId;
            model.Accomodations = entity.Accomodations;

            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.Contact = entity.Contact;            
            return model;
        }
        internal static CustomerEntity ToEntity(CustomerModel model)
        {
            CustomerEntity entity = new CustomerEntity();
            entity.CustomerId = model.CustomerId;
            entity.Accomodations = model.Accomodations;

            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Contact = model.Contact;
            return entity;
        }

        public Task<Dictionary<int, CustomerModel>> GetAllAsync()
        {
            var customers = dbContext.Customers.AsQueryable().ToDictionaryAsync(x => x.CustomerId, x => ToModel(x));
            return customers;
        }

        public async Task<CustomerModel> TryGetAsync(int id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            return ToModel(customer);
        }

        public async Task<PageResult<CustomerModel>> GetPageAsync(int page, int itemsPerPage)
        {
            var query = dbContext.Customers.AsQueryable();          
            var recordsAndTotalCount = await query.Select(p => new {
                Record = p,
                TotalCount = query.Count()
            }).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

            var result = recordsAndTotalCount.FirstOrDefault();
            int totalCount = 0;
            IEnumerable<CustomerModel> foundRecords = null;

            if (result != null)
            {
                totalCount = result.TotalCount;
                foundRecords = recordsAndTotalCount.Select(r => ToModel(r.Record));
            }
            return new PageResult<CustomerModel>(foundRecords, itemsPerPage, totalCount, page, (int)Math.Ceiling((double)totalCount / itemsPerPage));

        }

        public async Task<PageResult<CustomerModel>> GetPageByPatternAsync(int page, int itemsPerPage, string searchedPattern)
        {
            searchedPattern = $"%{searchedPattern}%";
            var query = dbContext.Customers.AsQueryable().Where(x => EF.Functions.Like(x.FirstName, searchedPattern) ||
            EF.Functions.Like(x.FirstName, searchedPattern) ||
            EF.Functions.Like(x.LastName, searchedPattern) ||
            EF.Functions.Like(x.Contact.Email, searchedPattern) ||
            EF.Functions.Like(x.FirstName + " " + x.LastName, searchedPattern) ||
            EF.Functions.Like(x.LastName + " " + x.FirstName, searchedPattern));
            var recordsAndTotalCount = await query.Select(p => new {
                Record = p,
                TotalCount = query.Count()
            }).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

            var result = recordsAndTotalCount.FirstOrDefault();
            int totalCount = 0;
            IEnumerable<CustomerModel> foundRecords = null;
            if (result != null)
            {
                totalCount = result.TotalCount;
                foundRecords = recordsAndTotalCount.Select(r => ToModel(r.Record));
            }
            return new PageResult<CustomerModel>(foundRecords, itemsPerPage, totalCount, page, (int)Math.Ceiling((double)totalCount / itemsPerPage));

        }

        public CustomerModel Update(CustomerModel model)
        {
            CustomerEntity entity = ToEntity(model);
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Modified;

            return model;
        }

        public void Remove(int id)
        {
            CustomerEntity entity = new CustomerEntity { CustomerId = id };
            entity.IsDeleted = true;
            dbContext.Attach(entity);
            dbContext.Entry(entity).Property(x => x.IsDeleted).IsModified = true;
        }

        public CustomerModel AddAsync(CustomerModel model)
        {
            throw new NotImplementedException();
        }

      

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
