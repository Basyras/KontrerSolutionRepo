using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.CustomerService.Data.Customer.EntityFramework;
using Kontrer.OwnerServer.CustomerService.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Accommodation.EntityFramework
{
    public class EfAccommodationRepository : IAccommodationRepository
    {
        private readonly CustomerServiceDbContext dbContext;

        public EfAccommodationRepository(CustomerServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        internal static FinishedAccommodationModel ToModel(FinishedAccommodationEntity entity)
        {
            FinishedAccommodationModel model = new FinishedAccommodationModel();
            model.AccommodationId = entity.AccommodationId;
            model.CustomerId = entity.CustomerId;
            model.Cost = entity.Cost;
            
            model.OwnersPrivateNotes = entity.Notes;
            return model;


        }
        internal static List<FinishedAccommodationModel> ToModels(IEnumerable<FinishedAccommodationEntity> entities)
        {
            List<FinishedAccommodationModel> models = new List<FinishedAccommodationModel>();
            foreach (var entity in entities)
            {
                var model = ToModel(entity);
                models.Add(model);
            }
            return models;
        }

        internal static FinishedAccommodationEntity ToEntity(FinishedAccommodationModel model)
        {
            FinishedAccommodationEntity entity = new FinishedAccommodationEntity()
            {
                AccommodationId = model.AccommodationId,                
                Cost = model.Cost,                             
                Notes = model.OwnersPrivateNotes
            };
            return entity;
        }


        public void Remove(int id)
        {
            FinishedAccommodationEntity entity = new FinishedAccommodationEntity()
            {
                AccommodationId = id,
            };

            dbContext.Accommodations.Remove(entity);
            
        }

     

        public void Add(FinishedAccommodationModel model)
        {
            var entity = ToEntity(model);
            dbContext.Accommodations.Add(entity);
        }

        public void Edit(FinishedAccommodationModel model)
        {
            FinishedAccommodationEntity entity = ToEntity(model);
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public Task<Dictionary<int, FinishedAccommodationModel>> GetAllAsync()
        {
            var accommodations = dbContext.Accommodations.AsQueryable().ToDictionaryAsync(x => x.AccommodationId, x => ToModel(x));
            return accommodations;
        }


        public async Task<FinishedAccommodationModel> GetAsync(int id)
        {
            var customer = await dbContext.Set<FinishedAccommodationModel>().FindAsync(id);
            return customer;

        }

        public async Task<PageResult<FinishedAccommodationModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern)
        {
            //searchedPattern = $"%{searchedPattern}%";
            //var query = dbContext.Accommodations.AsQueryable().Where(x => EF.Functions.Like(x.Customer.FirstName, searchedPattern) ||
            //EF.Functions.Like(x.Customer.LastName, searchedPattern) ||
            //EF.Functions.Like(x.Customer.LastName, searchedPattern) ||
            //EF.Functions.Like(x.Customer.Contact.Email, searchedPattern) ||
            //EF.Functions.Like(x.Customer.FirstName + " " + x.Customer.LastName, searchedPattern) ||
            //EF.Functions.Like(x.Customer.LastName + " " + x.Customer.FirstName, searchedPattern));


            var query = dbContext.Accommodations.AsQueryable();

            var recordsAndTotalCount = await query.Select(p => new
            {
                Record = p,
                TotalCount = query.Count()
            }).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

            var result = recordsAndTotalCount.FirstOrDefault();
            int totalCount = 0;
            IEnumerable<FinishedAccommodationModel> foundRecords = null;
            if (result != null)
            {
                totalCount = result.TotalCount;
                foundRecords = recordsAndTotalCount.Select(r => ToModel(r.Record));
            }
            return new PageResult<FinishedAccommodationModel>(foundRecords, itemsPerPage, totalCount, page, (int)Math.Ceiling((double)totalCount / itemsPerPage));




        }
    }
}
