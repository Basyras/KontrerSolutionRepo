using Basyc.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints
{
    public class PersonBlueprint
    {
        public PersonBlueprint(List<ItemBlueprint> personItems, List<DiscountBlueprint> discounts, PersonTypes personType)
        {
            PersonItems = personItems ?? new List<ItemBlueprint>();
            Discounts = discounts ?? new List<DiscountBlueprint>();
            PersonType = personType;
        }

        public PersonBlueprint(PersonTypes personType) : this(null, null, personType)
        {
        }

        public PersonBlueprint()
        {
        }

        public List<ItemBlueprint> PersonItems { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public PersonTypes PersonType { get; set; }
    }
}