using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing
{

    public class PricingResolver : IPricingSettingsResolver
    {
        private readonly IDictionary<string, NullableResult<object>> dic;

        public PricingResolver(IDictionary<string, NullableResult<object>> dic)
        {
            this.dic = dic;
        }

        //public async Task<TSetting> ResolveSettingValue<TSetting>(string settingUniqueName, DateTime? start = null, DateTime? end = null)
        //{
        //    var timedSetting = await repository.GetTimedSetting(settingUniqueName, start,end);
        //    return (TSetting)timedSetting.Value;
        //}


        //public Task<NullableResult<TSetting>> ResolveValueAsync<TSetting>(string settingUniqueName, DateTime? start = null, DateTime? end = null)
        //{
        //    var timedSettingTask = repository.GetTimedSetting<TSetting>(settingUniqueName, start, end);
        //    return timedSettingTask.ContinueWith((t) => 
        //    {                
        //        return t.Result; 
        //    });
        //}
        public NullableResult<TSetting> ResolveValue<TSetting>(string settingUniqueName)
        {
            var result = dic[settingUniqueName];
            return new NullableResult<TSetting>((TSetting)result.Value,result.WasFound,(TSetting)result.DefaultValue);
        }
    }
}
