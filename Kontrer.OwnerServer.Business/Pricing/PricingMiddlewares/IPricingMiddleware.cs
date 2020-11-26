namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public interface IPricingMiddleware<TBlueprint,TCost>
    {        
        string WorkDescription { get; }
        void CalculateContractCost(TBlueprint blueprint, ref TCost rawAccommodation, IPricingSettingsResolver resolver);
    }
}