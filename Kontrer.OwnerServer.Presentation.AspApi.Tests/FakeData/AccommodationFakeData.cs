using Bogus;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi.Tests.FakeData
{
    public static class AccommodationFakeData
    {
        public static List<AccommodationModel> GetAccommodationsWithoutCustomers(int count)
        {
            if (count == 0)
                return new List<AccommodationModel>();

            List<AccommodationModel> accommodations = GetAccommodationsWithoutCustomer(count);
            return accommodations;
        }



        public static List<AccommodationModel> GetAccommodationsWithoutCustomer(int count, bool hasSharedCurrencies = true, Currencies? sharedCurrency = null)
        {
            sharedCurrency ??= new Faker().Random.Enum<Currencies>();

            if (count == 0)
                return new List<AccommodationModel>();

            int idCounter = 0;
            var accommodations = new Faker<AccommodationModel>()
                .StrictMode(true)
                .RuleFor(x => x.Blueprint, (Faker x) => BlueprintFakeData.GetAccommodationBlueprints(1, true, sharedCurrency)[0])
                .RuleFor(x => x.AccomodationId, x => idCounter++)
                .RuleFor(x => x.Customer, x => null)
                .RuleFor(x => x.Cost, (Faker x, AccommodationModel a) => CostFakeData.GetAccommodationCosts(1, sharedCurrency)[0] with { DateFrom = a.Blueprint.Start, DateTo = a.Blueprint.End })
                .FinishWith((x, a) =>
                {
                    sharedCurrency = hasSharedCurrencies ? sharedCurrency : x.Random.Enum<Currencies>();
                })
                .Generate(count);

            return accommodations;
        }




    }
}
