using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public interface IPricer<TBlueprint,TCost> : IPricingMiddleware
    {              
        void CalculateContractCost(TBlueprint blueprint, TCost rawAccommodation, ITimedSettingResolver resolver);
        List<TimedSettingSelector> GetRequiredSettings(TBlueprint blueprint);
    }

    public interface IPricingMiddleware
    {
        /// <summary>
        /// 0 - does not have priority, higher = executed later
        /// </summary>
        int QueuePosition { get; }
        string WorkDescription { get; }
    }
}