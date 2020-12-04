using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public class ResolveRequest<TValue>
    {
        public ResolveRequest(string uniqueSettingName)
        {
            UniqueSettingName = uniqueSettingName;
        }

        public string UniqueSettingName { get; }
    }
}
