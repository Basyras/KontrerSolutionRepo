﻿using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models
{
    public class FinishedAccommodationModel
    {
        public FinishedAccommodationModel()
        {

        }

        public FinishedAccommodationModel(int accommodationId, int customerId, int orderId, AccommodationCost cost, AccommodationOrder order, string ownersPrivateNotes = null)
        {
            AccommodationId = accommodationId;
            CustomerId = customerId;
            Cost = cost;
            OwnersPrivateNotes = ownersPrivateNotes;
            OrderId = orderId;
        }

        public int AccommodationId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public AccommodationBlueprint Blueprint { get; set; }
        public AccommodationCost Cost { get; set; }
        public string OwnersPrivateNotes { get; set; }


    }
}
