using Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Business.Pricing.BlueprintEditors
{
    public interface IBlueprintEditor<TBlueprint> where TBlueprint : class
    {
        void EditBlueprint(TBlueprint blueprint, ITimedSettingResolver resolver);
        List<TimedSettingSelector> GetRequiredSettings(TBlueprint blueprint);
    }
}
