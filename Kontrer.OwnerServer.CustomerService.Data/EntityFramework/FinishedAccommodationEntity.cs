using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;

namespace Kontrer.OwnerServer.CustomerService.Data.EntityFramework
{
    public class FinishedAccommodationEntity
    {
        public int AccommodationId { get; set; }
        //public CustomerEntity Customer { get; set; }
        public int CustomerId { get; set; }
        public AccommodationCost Cost { get; set; }
        public AccommodationOrder Order { get; set; }                
        public string Notes { get; set; }
    }
}