﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.PricingServer.Data.Pricing.EntityFramework
{
    public class PricingSettingGroupEntity
    {
        [Key]
        public int PricingSettingGroupId { get; set; }
        public string SettingName { get; set; }
        public Type Type { get; set; }
        public List<PricingTimedSettingEntity> TimedSettings { get; set; } = new List<PricingTimedSettingEntity>();
     
    }
}