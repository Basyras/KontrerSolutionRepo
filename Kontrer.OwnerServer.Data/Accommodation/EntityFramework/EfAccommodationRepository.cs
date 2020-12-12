using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.Customer.EntityFramework;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Accommodation.EntityFramework
{
    public class EfAccommodationRepository : IAccommodationRepository
    {
        private readonly OwnerServerDbContext dbContext;

        public EfAccommodationRepository(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        internal static AccommodationModel ToModel(AccommodationEntity entity)
        {
            AccommodationModel model = new AccommodationModel();
            model.AccommodationId = entity.AccommodationId;
            model.Blueprint = entity.Blueprint;
            model.Cost = entity.Cost;
            model.OwnerNotes = entity.OwnerNotes;
            model.CreationTime = entity.CreationTime;
            model.Customer = EfCustomerRepository.ToModel(entity.Customer);
            model.State = entity.State;
            return model;


        }
        internal static List<AccommodationModel> ToModels(IEnumerable<AccommodationEntity> entities)
        {
            List<AccommodationModel> models = new List<AccommodationModel>();
            foreach (var entity in entities)
            {
                var model = ToModel(entity);
                models.Add(model);
            }
            return models;
        }
        internal static AccommodationEntity ToEntity(AccommodationModel model)
        {
            AccommodationEntity entity = new AccommodationEntity()
            {
                AccommodationId = model.AccommodationId,
                Blueprint = model.Blueprint,
                Cost = model.Cost,
                CreationTime = model.CreationTime,
                Customer = EfCustomerRepository.ToEntity(model.Customer),
                OwnerNotes = model.OwnerNotes,
                State = model.State

            };

            return entity;

        }


        public void Cancel(int id, bool canceledByCustomer, string notes = null)
        {
            AccommodationEntity entity = new AccommodationEntity()
            {
                AccommodationId = id,
                State = AccommodationState.CanceledByCustomer,
                OwnerNotes = notes
            };

            dbContext.Accommodations.Attach(entity);
            dbContext.Entry(entity).Property(x => x.State).IsModified = true;
            dbContext.Entry(entity).Property(x => x.OwnerNotes).IsModified = true;
        }

        public void Complete(int id)
        {
            AccommodationEntity entity = new AccommodationEntity()
            {
                AccommodationId = id,
                State = AccommodationState.Completed

            };
            dbContext.Accommodations.Attach(entity);
            dbContext.Entry(entity).Property(x => x.State).IsModified = true;

        }

        public void Create(int customerId, AccommodationCost cost, AccommodationBlueprint blueprint)
        {
            AccommodationEntity entity = new AccommodationEntity()
            {
                AccommodationId = customerId,
                Cost = cost,
                Blueprint = blueprint
            };
            dbContext.Accommodations.Add(entity);
        }

        public void Edit(AccommodationModel model)
        {
            AccommodationEntity entity = ToEntity(model);
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public Task<Dictionary<int, AccommodationModel>> GetAllAsync()
        {
            var accommodations = dbContext.Accommodations.AsQueryable().ToDictionaryAsync(x => x.AccommodationId, x => ToModel(x));
            return accommodations;

        }


        public async Task<AccommodationModel> GetAsync(int id)
        {
            var customer = await dbContext.Set<AccommodationModel>().FindAsync(id);
            return customer;

        }

        public async Task<PageResult<AccommodationModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern)
        {
            searchedPattern = $"%{searchedPattern}%";
            var query = dbContext.Accommodations.AsQueryable().Where(x => EF.Functions.Like(x.Customer.FirstName, searchedPattern) ||
            EF.Functions.Like(x.Customer.SecondName, searchedPattern) ||
            EF.Functions.Like(x.Customer.SecondName, searchedPattern) ||
            EF.Functions.Like(x.Customer.Email, searchedPattern) ||
            EF.Functions.Like(x.Customer.FirstName + " " + x.Customer.SecondName, searchedPattern) ||
            EF.Functions.Like(x.Customer.SecondName + " " + x.Customer.FirstName, searchedPattern));
            var recordsAndTotalCount = await query.Select(p => new{
            Record = p,
            TotalCount = query.Count()
             }).Skip((page-1)*itemsPerPage).Take(itemsPerPage).ToListAsync();

            var result = recordsAndTotalCount.FirstOrDefault();
            int totalCount = 0;
            IEnumerable<AccommodationModel> foundRecords = null;
            if(result!=null)
            {
                totalCount = result.TotalCount;
                foundRecords = recordsAndTotalCount.Select(r => ToModel(r.Record));
            }
            return new PageResult<AccommodationModel>(foundRecords, itemsPerPage, totalCount, page, (int)Math.Ceiling((double)totalCount / itemsPerPage));


           

        }
    }
}
