using System;

namespace Kontrer.OwnerServer.OfferService.Domain
{
    public class Offer
    {
        public Offer(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}