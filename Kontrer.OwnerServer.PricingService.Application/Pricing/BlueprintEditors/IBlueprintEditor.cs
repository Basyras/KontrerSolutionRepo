using Kontrer.OwnerServer.PricingService.Infrastructure.Abstraction.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Pricing.BlueprintEditors
{
    /// <summary>
    /// Prepares blueprint before calculating the price. Settings default values, discounts, etc.
    /// </summary>
    /// <typeparam name="TBlueprint"></typeparam>
    public interface IBlueprintEditor<TBlueprint> where TBlueprint : class
    {
        void EditBlueprint(TBlueprint blueprint, ITimedSettingResolver resolver);
        List<TimedSettingSelector> GetRequiredSettings(TBlueprint blueprint);
    }
}
