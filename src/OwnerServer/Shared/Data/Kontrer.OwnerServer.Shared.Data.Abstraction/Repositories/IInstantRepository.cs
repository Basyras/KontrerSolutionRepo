﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    /// <summary>
    /// Performs actions instatly when called. Repository should not have save/commint/complete implementation
    /// <br/>
    /// Choose between <see cref="IBulkRepository"/>, <see cref="IInstantCrudRepository{TModel, TKey}"/> or <see cref="IInstantRepository"/>.
    /// </summary>
    public interface IInstantRepository
    {

    }
}