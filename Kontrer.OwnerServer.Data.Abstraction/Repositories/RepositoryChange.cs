using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Repositories
{
    public class RepositoryChange<TModel, TKey> where TModel : class
    {
        //public PriceChangedSettingModel()
        //{

        //}

        public RepositoryChange(TKey id, TModel model, PriceChangedActions action)
        {
            Action = action;
            Model = model;
            Id = id;
        }

        public PriceChangedActions Action { get; set; }
        public TModel Model { get; set; }
        public TKey Id { get; set; }
    }

  
}
