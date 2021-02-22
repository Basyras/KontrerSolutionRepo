using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Shared.Data.Abstraction.UnitOfWork;
using Kontrer.Shared.Models;


namespace Kontrer.OwnerServer.CustomerService.Business.Abstraction.Accommodations
{
    public interface IAccommodationManager
    {
        IAccommodationUnitOfWork CreateUnitOfWork();        
    }
}
