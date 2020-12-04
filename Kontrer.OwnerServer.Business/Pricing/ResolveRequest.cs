using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public class ResolveRequest<TValue>
    {
        public ResolveRequest(string uniqueSettingName, DateTime? start, DateTime? end )
        {
            UniqueSettingName = uniqueSettingName;
            Start = start;
            End = end;
        }

        public ResolveRequest(string uniqueSettingName)
        {
            UniqueSettingName = uniqueSettingName;
        }

        public string UniqueSettingName { get; }
        public DateTime? Start { get; }
        public DateTime? End { get; }
    }
}
