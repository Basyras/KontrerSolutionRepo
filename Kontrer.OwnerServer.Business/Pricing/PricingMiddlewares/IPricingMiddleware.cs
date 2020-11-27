﻿namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public interface IPricingMiddleware<TBlueprint,TCost> : IPricingMiddleware
    {              
        void CalculateContractCost(TBlueprint blueprint, ref TCost rawAccommodation, IPricingSettingsResolver resolver);        
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