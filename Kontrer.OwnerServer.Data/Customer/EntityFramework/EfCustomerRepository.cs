using Kontrer.OwnerServer.Data.Abstraction.Customer;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Customer.EntityFramework
{
    public class EfCustomerRepository : ICustomerRepository
    {
        private readonly OwnerServerDbContext dbContext;

        public EfCustomerRepository(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        internal static CustomerModel ToModel(CustomerEntity entity)
        {
            CustomerModel model = new CustomerModel();
            model.Accomodations = entity.Accomodations;
            model.CustomerId = entity.CustomerId;
            model.Email = entity.Email;
            model.FirstName = entity.FirstName;
            model.PhoneNumber = entity.PhoneNumber;
            model.SecondName = entity.SecondName;
            return model;
        }
        internal static CustomerEntity ToEntity(CustomerModel model)
        {
            CustomerEntity entity = new CustomerEntity();
            entity.Accomodations = model.Accomodations;
            entity.CustomerId = model.CustomerId;
            entity.Email = model.Email;
            entity.FirstName = model.FirstName;
            entity.PhoneNumber = model.PhoneNumber;
            entity.SecondName = model.SecondName;
            return entity;
        }

        public Task<Dictionary<int, CustomerModel>> GetAllAsync()
        {
            var customers = dbContext.Customers.AsQueryable().ToDictionaryAsync(x => x.CustomerId, x => ToModel(x));
            return customers;
        }

        public async Task<CustomerModel> GetAsync(int id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            return ToModel(customer);
        }

        public async Task<PageResult<CustomerModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern)
        {
            searchedPattern = $"%{searchedPattern}%";
            var query = dbContext.Customers.AsQueryable().Where(x => EF.Functions.Like(x.FirstName, searchedPattern) ||
            EF.Functions.Like(x.SecondName, searchedPattern) ||
            EF.Functions.Like(x.SecondName, searchedPattern) ||
            EF.Functions.Like(x.Email, searchedPattern) ||
            EF.Functions.Like(x.FirstName + " " + x.SecondName, searchedPattern) ||
            EF.Functions.Like(x.SecondName + " " + x.FirstName, searchedPattern));
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

        public void Edit(CustomerModel model)
        {
            CustomerEntity entity = ToEntity(model);
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public void Remove(int id)
        {
            CustomerEntity entity = new CustomerEntity { CustomerId = id };
            entity.IsDeleted = true;
            dbContext.Attach(entity);
            dbContext.Entry(entity).Property(x => x.IsDeleted).IsModified = true;
        }

        public void Save()
        {
            //dbContext.SaveChanges();
            throw new NotImplementedException();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            //await dbContext.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //dbContext.Dispose();
            throw new NotImplementedException();
        }
        
    }
}
