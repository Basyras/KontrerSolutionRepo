using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models
{
    public class AccommodationModel
    {
        public AccommodationModel()
        {

        }

        public int AccommodationId { get; set; }
        public CustomerModel Customer { get; set; }
        public AccommodationCost Cost { get; set; }
        public AccommodationBlueprint Blueprint { get; set; }
        public DateTime CreationTime { get; set; }
        public string OwnerNotes { get; set; }
        public AccommodationState State { get; set; }


    }
}
