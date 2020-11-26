using Bogus;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi.Tests.FakeData
{
    public static class CostFakeData
    {
        public static List<AccommodationCost> GetAccommodationCosts(int count, Currencies? sharedCurrency =null)
        {

            sharedCurrency ??= new Faker().Random.Enum<Currencies>();
            
            if (count == 0)
            {
                return new List<AccommodationCost>();
            }

            var costs = new Faker<AccommodationCost>()
                .StrictMode(true)
                .CustomInstantiator(x => new AccommodationCost(
                    default,
                    default,
                    default,
                    default,
                    default,
                    null))

                  .RuleFor(x => x.DateFrom, (Faker x) => x.Date.Recent(15, x.Date.Between(default, DateTime.MaxValue)))
                  .RuleFor(x => x.DateTo, (Faker x) => x.Date.Soon(15, x.Date.Between(default, DateTime.MaxValue)))
                  .RuleFor(x => x.AccomodationItems, (Faker x, AccommodationCost a) => CostFakeData.GetItemCosts(x.Random.Number(0, 5),true, sharedCurrency))
                  .RuleFor(x => x.Rooms, (Faker x) => GetRoomCosts(x.Random.Int(0, 5), sharedCurrency))
                  .RuleFor(x => x.Customer, x => null)
                  .RuleFor(x => x.TotalCost, (Faker x, AccommodationCost r) =>
                  {                     
                      return new Cash(sharedCurrency.Value, r.AccomodationItems.Sum(x => x.CostPerOne.Amout) + r.Rooms.Sum(x => x.TotalCost.Amout));
                  })

                .Generate(count);

            return costs;
        }

        public static List<ItemCost> GetItemCosts(int count, bool allCurrenciesSame = true, Currencies? sharedCurrency = null)
        {
            sharedCurrency ??= new Faker().Random.Enum<Currencies>();

            if (count == 0)
            {
                return new List<ItemCost>();
            }

            var costs = new Faker<ItemCost>()
                .StrictMode(true)
                .CustomInstantiator(x => new ItemCost(default, default, default, default))
                .RuleFor(x => x.Name, (Faker x) => x.Random.Words(x.Random.Int(1, 5)))
                .RuleFor(x => x.CostPerOne, (Faker x) => new Cash(sharedCurrency.Value, x.Random.Float(0, 1000)))
                .RuleFor(x => x.Descriptions, (Faker x) => new Dictionary<string, string>(new Faker<Tuple<string, string>>()
                        .CustomInstantiator(x => new Tuple<string, string>(x.IndexGlobal.ToString(), ""))
                       //.RuleFor(x => x.Item1, (Faker x) => x.Random.Word()) ///ffffgdfguifguguuguduflkad ou  iojqwd jioiiojasdadasdsdtrioiiiasdoij iiojasdasddddasdasdasdasd
                       .RuleFor(x => x.Item2, (Faker x) => x.Random.Words(x.Random.Int(0, 5)))
                       .Generate(x.Random.Int(0, 10)).Select(x => new KeyValuePair<string, string>(x.Item1, x.Item2))))
                .RuleFor(x=>x.Count,x=> x.Random.Int(0,5))
                .FinishWith((x, a) => 
                {
                    sharedCurrency = allCurrenciesSame ? sharedCurrency : new Faker().Random.Enum<Currencies>();
                })
                .Generate(count);

            return costs;
        }

        public static List<RoomCost> GetRoomCosts(int count,Currencies? sharedCurrency = null)
        {

            sharedCurrency ??= new Faker().Random.Enum<Currencies>();
            if (count == 0)
            {
                return new List<RoomCost>();
            }


            var rooms = new Faker<RoomCost>()
                .StrictMode(true)
                .CustomInstantiator(x => new RoomCost(null, null, null))
                .RuleFor(x => x.PersonCosts, x => GetPersonCosts(x.Random.Int(0, 5)))
                .RuleFor(x => x.RoomItems, (Faker x) => GetItemCosts(x.Random.Int(0, 5),true, sharedCurrency))
                .RuleFor(x => x.TotalCost, (Faker x, RoomCost r) =>
                   {
                       Currencies cur = r.RoomItems.Count == 0 ? x.Random.Enum<Currencies>() : r.RoomItems[0].CostPerOne.Currency;
                       return new Cash(cur, r.RoomItems.Sum(x => x.CostPerOne.Amout));
                   })
                .Generate(count);

            return rooms;
        }

        public static List<PersonCost> GetPersonCosts(int count)
        {

            if (count == 0)
            {
                return new List<PersonCost>();
            }

            var people = new Faker<PersonCost>()
                .StrictMode(true)
                .CustomInstantiator(x => new PersonCost(null, null))
                .RuleFor(x => x.Items, (Faker x) => GetItemCosts(x.Random.Int(0, 5)))
                   .RuleFor(x => x.TotalCost, (Faker x, PersonCost r) =>
                   {
                       Currencies cur = r.Items.Count == 0 ? x.Random.Enum<Currencies>() : r.Items[0].CostPerOne.Currency;
                       return new Cash(cur, r.Items.Sum(x => x.CostPerOne.Amout));
                   })
                .Generate(count);
            return people;
        }
    }
}
