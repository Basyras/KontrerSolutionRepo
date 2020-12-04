using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Data.Abstraction.UnitOfWork;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Accommodation
{
    public interface IAccommodationUnitOfWork : IUnitOfWork
    {
        IAccommodationRepository Accommodations { get; }
    }
}
