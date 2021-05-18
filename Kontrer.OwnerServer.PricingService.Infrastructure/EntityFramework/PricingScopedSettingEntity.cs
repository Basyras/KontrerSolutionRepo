using System;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{
    public class PricingScopedSettingEntity
    {
      
        public PricingSettingEntity Setting { get; set; }
      
        public string SettingId { get; set; }
        
        public PricingSettingTimeScopeEntity TimeScope { get; set; }
     
        public int TimeScopeId { get; set; }
        /// <summary>
        /// Serialized setting value
        /// </summary>
        public string Value { get; set; } 
    }
}