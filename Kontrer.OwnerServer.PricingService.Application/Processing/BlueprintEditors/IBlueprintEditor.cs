using Kontrer.OwnerServer.PricingService.Application.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors
{
    /// <summary>
    /// Prepares blueprint before calculating the price. Settings default values, discounts, etc.
    /// </summary>
    /// <typeparam name="TBlueprint"></typeparam>
    public interface IBlueprintEditor<TBlueprint> where TBlueprint : class
    {
        void EditBlueprint(TBlueprint blueprint, IScopedSettings settings);
        List<SettingRequest> GetRequiredSettings(TBlueprint blueprint);
    }
}
