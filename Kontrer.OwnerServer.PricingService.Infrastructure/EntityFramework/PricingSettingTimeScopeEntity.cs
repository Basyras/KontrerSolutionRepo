﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingSettingTimeScopeEntity
    {        
        [Key]
        public int PricingSettingScopeEntityId { get; set; }        
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<PricingScopedSettingEntity> ScopedSettings { get; set; }
      
    }
}