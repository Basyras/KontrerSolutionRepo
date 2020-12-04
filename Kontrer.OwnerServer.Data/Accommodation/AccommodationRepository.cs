using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.Customer;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Accommodation
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly OwnerServerDbContext dbContext;

        public AccommodationRepository(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public static AccommodationModel ToModel(AccommodationEntity entity)
        {
            AccommodationModel model = new AccommodationModel();
            model.AccommodationId = entity.AccommodationId;
            model.Blueprint = entity.Blueprint;
            model.Cost = entity.Cost;
            model.Notes = entity.Notes;
            model.CreationTime = entity.CreationTime;
            model.Customer = CustomerRepository.ToModel(entity.Customer);
            model.State = entity.State;
            return model;


        }
        public static List<AccommodationModel> ToModels(IEnumerable<AccommodationEntity> entities)
        {
            List<AccommodationModel> models = new List<AccommodationModel>();
            foreach (var entity in entities)
            {
                var model = ToModel(entity);
                models.Add(model);
            }
            return models;
        }
        public static AccommodationEntity ToEntity(AccommodationModel model)
        {
            AccommodationEntity entity = new AccommodationEntity()
            {
                AccommodationId = model.AccommodationId,
                Blueprint = model.Blueprint,
                Cost = model.Cost,
                CreationTime = model.CreationTime,
                Customer = CustomerRepository.ToEntity(model.Customer),
                Notes = model.Notes,
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
                Notes = notes
            };

            dbContext.Accommodations.Attach(entity);
            dbContext.Entry(entity).Property(x => x.State).IsModified = true;
            dbContext.Entry(entity).Property(x => x.Notes).IsModified = true;


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

        public Task<PageResult<AccommodationModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
