using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing.Blueprints
{
    public class PersonBlueprint
    {
        public List<ItemBlueprint> PersonItems { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public PersonTypes PersonType { get; set; }

    }
}
