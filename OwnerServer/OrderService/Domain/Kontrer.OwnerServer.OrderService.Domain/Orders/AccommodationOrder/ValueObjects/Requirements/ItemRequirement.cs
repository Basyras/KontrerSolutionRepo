using Basyc.Shared.Models.Pricing;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements
{
    public class ItemRequirement
    {
        public int? ItemId { get; set; }

        public string ItemName { get; set; }
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