﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing.Blueprints
{
    
    public class DiscountBlueprint
    {
        public DiscountBlueprint()
        {

        }

        public DiscountBlueprint(string discountName, Cash amountDiscount, int? discountId = null)
        {
            DiscountName = discountName;            
            AmountDiscount = amountDiscount;
            DiscountId = discountId;
            IsPercentage = false;
        }

        public DiscountBlueprint(string discountName, float percentageDiscount, int? discountId = null)
        {
            DiscountName = discountName;
            PercentageDiscount = percentageDiscount;
            DiscountId = discountId;
            IsPercentage = true;
        }



        public int? DiscountId { get; set; }
        public string DiscountName { get; set; }
        public bool IsPercentage { get; set; }
        public Cash AmountDiscount { get; set; }
        public float PercentageDiscount { get; set; }
    }
}