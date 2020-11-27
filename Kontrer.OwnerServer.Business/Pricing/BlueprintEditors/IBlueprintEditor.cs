using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing.BlueprintEditors
{
    public interface IBlueprintEditor<TBlueprint>
    {
        void EditBlueprint(ref TBlueprint blueprint, IPricingSettingsResolver resolver);
    }
}
