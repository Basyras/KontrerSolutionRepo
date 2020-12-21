using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation
{
    public interface IAccommodationUnitOfWork : IUnitOfWork
    {
        IAccommodationRepository Accommodations { get; }
    }
}
