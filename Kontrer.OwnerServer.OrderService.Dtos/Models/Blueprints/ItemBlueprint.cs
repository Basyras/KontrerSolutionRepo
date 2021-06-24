using Kontrer.Shared.Models.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints
{
    public class ItemBlueprint
    {
        [Obsolete("Workaround for bogus", true)]
        public ItemBlueprint() //Workaround for bogus
        {
        }

        public ItemBlueprint(Cash costPerOne, int count, float taxPercentageToAdd, string itemName)
        {
            CostPerOne = costPerOne;
            TaxPercentageToAdd = taxPercentageToAdd;
            Count = count;
            ItemName = itemName;
        }

        //Null when not saved, When not null it should load this item from persistance storage - to reflect any changes for this item
        public int? ItemId { get; set; }

        public string ItemName { get; set; }
        public Dictionary<string, string> ExtraInfo { get; set; }
        public Cash CostPerOne { get; set; }
        public int Count { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public bool CanParentApplyDiscount { get; set; }

        /// <summary>
        /// 100% is 1.00F
        /// </summary>
        public float TaxPercentageToAdd { get; set; }
    }
}