using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Pricing
{
    public class DefaultPricingSettingsRepository : IPricingSettingsRepository
    {
        public List<RepositoryChange<PricingSettingsModel, int>> Changes => throw new NotImplementedException();

        public void Add(int id, PricingSettingsModel model)
        {
           //throw new NotImplementedException();
           //dbContext.Settings.Add();
           
        }

        public Task<Dictionary<int, PricingSettingsModel>> GetAllAsync()
        {
            throw new NotImplementedException();
            //dbContext.Settings.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void TryAdd(int id, PricingSettingsModel model)
        {
            throw new NotImplementedException();
        }

        public Task<PricingSettingsModel> TryGetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, PricingSettingsModel model)
        {
            throw new NotImplementedException();
        }
    }
}
