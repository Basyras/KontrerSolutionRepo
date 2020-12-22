using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;

namespace Kontrer.OwnerServer.CustomerService.Data.EntityFramework
{
    public class AccommodationEntity
    {
        public int AccommodationId { get; set; }
        public CustomerEntity Customer { get; set; }
        public AccommodationCost Cost { get; set; }
        public AccommodationBlueprint Blueprint { get; set; }
        public DateTime CreationTime { get; set; }
        public string OwnerNotes { get; set; }
        public AccommodationState State { get; set; }
       

    }
}