using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements
{
    public class PersonBlueprint
    {
        public PersonBlueprint(List<ItemRequirement> personItems, List<DiscountBlueprint> discounts, PersonTypes personType)
        {
            PersonItems = personItems ?? new List<ItemRequirement>();
            Discounts = discounts ?? new List<DiscountBlueprint>();
            PersonType = personType;
        }

        public PersonBlueprint(PersonTypes personType) : this(null, null, personType)
        {
        }

        public PersonBlueprint()
        {
        }

        public List<ItemRequirement> PersonItems { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public PersonTypes PersonType { get; set; }
    }
}