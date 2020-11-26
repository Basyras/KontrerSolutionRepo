using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing.Blueprints
{
    public class DiscountBlueprint
    {
        public int? DiscountId { get; set; }
        public string DiscountName { get; set; }
        public bool IsPercentage { get; set; }
        public Cash AmountDiscount { get; set; }
        public float PercentageDiscount { get; set; }
    }
}
