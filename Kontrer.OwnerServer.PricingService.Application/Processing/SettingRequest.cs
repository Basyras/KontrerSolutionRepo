using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing
{
    public class SettingRequest<TValue> : SettingRequest
    {
        public SettingRequest(string uniqueSettingName) : base(uniqueSettingName)
        {
        }

        //public SettingRequest(string uniqueSettingName, DateTime start, DateTime end) : base(uniqueSettingName,start,end,typeof(TValue))
        //{          
        //}
       
    }

    public class SettingRequest
    {
        public SettingRequest(string uniqueSettingName)
        {
            UniqueSettingName = uniqueSettingName;
        }

        public SettingRequest(string uniqueSettingName, Type settingType) : this(uniqueSettingName)
        {            
            SettingType = settingType;
        }

        public string UniqueSettingName { get; }
        public Type SettingType { get; }
    }
}
