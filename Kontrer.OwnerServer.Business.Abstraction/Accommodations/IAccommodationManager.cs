using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Business.Abstraction.UnitOfWork;
using Kontrer.Shared.Models;


namespace Kontrer.OwnerServer.Business.Abstraction.Accommodations
{
    public interface IAccommodationManager
    {

        IAccommodationUnitOfWork CreateUnitOfWork();        

    }
}
