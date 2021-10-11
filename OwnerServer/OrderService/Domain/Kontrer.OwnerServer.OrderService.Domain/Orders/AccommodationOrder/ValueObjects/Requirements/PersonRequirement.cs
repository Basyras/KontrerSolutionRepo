using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements
{
    public class PersonRequirement
    {
        public PersonRequirement(List<ItemRequirement> personItems, List<DiscountBlueprint> discounts, PersonTypes personType)
        {
            PersonItems = personItems ?? new List<ItemRequirement>();
            Discounts = discounts ?? new List<DiscountBlueprint>();
            PersonType = personType;
        }

        public PersonRequirement(PersonTypes personType) : this(null, null, personType)
        {
        }

        public PersonRequirement()
        {
        }

        public List<ItemRequirement> PersonItems { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public PersonTypes PersonType { get; set; }
    }
}