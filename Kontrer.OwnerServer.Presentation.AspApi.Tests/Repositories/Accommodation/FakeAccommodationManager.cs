using Kontrer.OwnerServer.Business.Abstraction.Accommodations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi.Tests.Repositories.Accommodation
{
    public class FakeAccommodationManager : IAccommodationManager
    {
        public IAccommodationUnitOfWork CreateUnitOfWork()
        {
            throw new NotImplementedException();
        }
    }
}
