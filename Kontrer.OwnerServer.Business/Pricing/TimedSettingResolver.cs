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

    public class TimedSettingResolver : ITimedSettingResolver
    {
        private readonly IDictionary<string, NullableResult<object>> dic;

        public TimedSettingResolver(IDictionary<string, NullableResult<object>> dic)
        {
            this.dic = dic;
        }

        public NullableResult<TSetting> ResolveValue<TSetting>(string settingUniqueName)
        {
            var result = dic[settingUniqueName];
            return new NullableResult<TSetting>((TSetting)result.Value,result.WasFound,(TSetting)result.DefaultValue);
        }
    }
}
