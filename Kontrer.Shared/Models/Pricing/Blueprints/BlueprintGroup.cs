﻿using System.Collections.Generic;

namespace Kontrer.Shared.Models.Pricing.Blueprints
{
    public class BlueprintGroup
    {
        public string GroupName { get; init; }
        public List<BlueprintGroup> Groups { get; set; } = new List<BlueprintGroup>();
        public List<ItemBlueprint> Items { get; set; } = new List<ItemBlueprint>();
        public List<DiscountBlueprint> Discounts { get; set; } = new List<DiscountBlueprint>();
    }
}