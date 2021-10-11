using Kontrer.OwnerServer.OfferService.Domain;
using Kontrer.Shared.Repositories;
using System;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.OfferService.Application
{
    public interface IOfferRepository : IAsyncInstantCrudRepository<Offer, int>
    {
    }
}