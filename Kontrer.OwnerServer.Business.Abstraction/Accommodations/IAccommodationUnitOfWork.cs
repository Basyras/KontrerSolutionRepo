using Kontrer.OwnerServer.Business.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Accommodations
{
    public interface IAccommodationUnitOfWork : IUnitOfWork
    {
        IAccommodationRepository Accommodations { get; }
    }
}
