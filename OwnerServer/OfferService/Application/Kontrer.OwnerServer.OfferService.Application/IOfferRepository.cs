using Kontrer.OwnerServer.OfferService.Domain;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.OfferService.Application
{
    public interface IOfferRepository : IInstantCrudRepository<Offer, int>
    {
    }
}