using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PdfCreatorService.Presentation.Abstract.Actors.PdfCreator
{
    public class AccommodationOfferViewModel
    {
        public AccommodationOfferViewModel()
        {

        }

        public AccommodationOfferViewModel(AccommodationCost cost, AccommodationBlueprint blueprint, TraderModel trader, string ownerPublicNotes, CustomerModel customer, int orderId)
        {
            Cost = cost;
            Blueprint = blueprint;
            Trader = trader;
            OwnerPublicNotes = ownerPublicNotes;
            Customer = customer;
            OrderId = orderId;
        }

        public AccommodationCost Cost { get; }
        public AccommodationBlueprint Blueprint { get; }
        public TraderModel Trader { get; }
        public CustomerModel Customer { get; }
        public int OrderId { get; }
        public string OwnerPublicNotes { get; }


    }
}
