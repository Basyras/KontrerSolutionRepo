using Bogus;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi.Tests.FakeData
{
    public static class BlueprintFakeData
    {
        public static List<RoomBlueprint> GetRoomBlueprints(int count, bool hasSharedCurrency = true, Currencies? sharedCurrency = null)
        {
            sharedCurrency ??= new Faker().Random.Enum<Currencies>();

            if (count == 0)
            {
                return new List<RoomBlueprint>();
            }

            var rooms = new Faker<RoomBlueprint>()
                .StrictMode(true)
                .RuleFor(x => x.RoomStartDate, (Faker x) => x.Date.Past(1, x.Date.Recent(x.Random.Int(0, 20), x.Date.Between(default, DateTime.MaxValue))))
                .RuleFor(x => x.RoomEndDate, (Faker x) => x.Date.Future(1, x.Date.Soon(x.Random.Int(0, 20), x.Date.Between(default, DateTime.MaxValue))))
                .RuleFor(x => x.People, x => GetPeopleBlueprints(x.Random.Int(0, 6),true, sharedCurrency))
                .RuleFor(x => x.RoomItems, x => GetItemBlueprints(x.Random.Int(0, 5),true, sharedCurrency))
                  .FinishWith((x, a) =>
                  {
                      sharedCurrency = hasSharedCurrency ? sharedCurrency : x.Random.Enum<Currencies>();
                  })
                .Generate(count);
            return rooms;
        }

        public static List<PersonBlueprint> GetPeopleBlueprints(int count, bool hasSharedCurrency = true, Currencies? sharedCurrency = null)
        {
            sharedCurrency ??= new Faker().Random.Enum<Currencies>();
            if (count == 0)
            {
                return new List<PersonBlueprint>();
            }

            var people = new Faker<PersonBlueprint>()
                .StrictMode(true)
                .RuleFor(x => x.PersonType, x => x.Random.Enum<PersonTypes>())
                .RuleFor(x => x.PersonItems, x => GetItemBlueprints(x.Random.Int(0, 8),true, sharedCurrency))
                  .FinishWith((x, a) =>
                  {
                      sharedCurrency = hasSharedCurrency ? sharedCurrency :x.Random.Enum<Currencies>();
                  })
                .Generate(count);
            return people;
        }

        public static List<ItemBlueprint> GetItemBlueprints(int count, bool hasSharedCurrency = true, Currencies? sharedCurrency = null)
        {
            sharedCurrency ??= new Faker().Random.Enum<Currencies>();

            if (count == 0)
            {
                return new List<ItemBlueprint>();
            }

            var items = new Faker<ItemBlueprint>()
                .StrictMode(true)
                .RuleFor(x => x.CanBeDiscounted, x => x.Random.Bool())
                .RuleFor(x => x.CostPerOne, x => new Cash(sharedCurrency.Value, x.Random.Float(0, 500)))
                .RuleFor(x => x.Count, x => x.Random.Int(0, 5))
                .RuleFor(x => x.ExtraInfo, (Faker x) => new Dictionary<string, string>(new Faker<Tuple<string, string>>()
                       .CustomInstantiator(x => new Tuple<string, string>(x.IndexGlobal.ToString(), ""))
                        //.RuleFor(x => x.Item1, (Faker x) => x.IndexGlobal.ToString())
                       .RuleFor(x => x.Item2, (Faker x) => x.Random.Words(x.Random.Int(0, 5)))
                       .Generate(x.Random.Int(0, 10)).Select(x => new KeyValuePair<string, string>(x.Item1, x.Item2))))
                .RuleFor(x => x.ItemId, (Faker x) => x.UniqueIndex)
                .RuleFor(x => x.ItemName, x => x.Random.Word())
                .RuleFor(x => x.TaxPercentageCut, x => x.Random.Int(0, 20))
                .FinishWith((x, a) =>
                {
                    sharedCurrency = hasSharedCurrency ? sharedCurrency :x.Random.Enum<Currencies>();
                })
                .Generate(count);
            return items;

        }

        public static List<AccommodationBlueprint> GetAccommodationBlueprints(int count, bool hasSharedCurrency = true, Currencies? sharedCurrency = null)
        {
            sharedCurrency ??= new Faker().Random.Enum<Currencies>();

            if (count == 0)
            {
                return new List<AccommodationBlueprint>();
            }

            var accos = new Faker<AccommodationBlueprint>()
                .StrictMode(true)
                .RuleFor(x => x.Deposit, x => x.Random.Bool() == true ? new Cash(sharedCurrency.Value, x.Random.Float(0, 500)) : null)
                .RuleFor(x => x.Start, (Faker x) => x.Date.Between(x.Date.Recent(3000), x.Date.Recent(3000)))
                .RuleFor(x => x.End, (Faker x, AccommodationBlueprint a) => x.Date.Soon(x.Random.Int(0, 30), a.Start))
                .RuleFor(x => x.Rooms, x => GetRoomBlueprints(x.Random.Int(0, 5),true,sharedCurrency))
                .RuleFor(x => x.DepositDeadline, (Faker x, AccommodationBlueprint a) =>
                   {
                       DateTime? rss = (a.Deposit == null) ? null : x.Date.Soon(15, a.End);
                       return rss;
                   })
                .RuleFor(x => x.ContractItems, (Faker x, AccommodationBlueprint a) => GetItemBlueprints(x.Random.Int(0, 5), true, sharedCurrency))
                .FinishWith((x, a) =>
                {
                    sharedCurrency = hasSharedCurrency ? sharedCurrency : x.Random.Enum<Currencies>();
                })
                .Generate(count);
            return accos;
        }
    }
}
