using Bogus;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi.Tests.FakeData
{
    public static class CustomerFakeData
    {
        public static List<Customer> GetCustomersWithAccommodation(int count)
        {

            if (count == 0)
            {
                return new List<Customer>();
            }

            var f = new Faker();

            var range = Enumerable.Range(1, 10000);
            var randomized = f.Random.Shuffle(range);
            var unique = randomized.Take(count);
            var enumerator = unique.GetEnumerator();


            var customers = new Faker<Customer>()
                //.RuleFor(x => x.CustomerId, x => idCounter++)
                .RuleFor(x => x.CustomerId, x =>
                {
                    enumerator.MoveNext();
                    return enumerator.Current;
                })
                .RuleFor(x => x.Accomodations, (Faker faker, Customer customer) =>
                   {
                       var accos = AccommodationFakeData.GetAccommodationsWithoutCustomer(faker.Random.Int(0,5));
                       foreach (var acco in accos)
                       {
                           acco.Customer = customer;
                           acco.Cost = CostFakeData.GetAccommodationCosts(1)[0] with { Customer = customer };
                       }

                       return accos;

                   })
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                .RuleFor(x => x.SecondName, x => x.Person.LastName)
                //.RuleFor(x=>x.PhoneNumber,x=>new Random().Next(000000000,99999999))                
                .RuleFor(x => x.PhoneNumber, (x) => int.Parse(x.Random.Replace("#########")))
                .Generate(count);
            return customers;
        }

        public static List<Customer> GetCustomersWithoutAccommodation(int count)
        {
            if (count == 0)
            {
                return new List<Customer>();
            }

            var f = new Faker();

            var range = Enumerable.Range(1, 10000);
            var randomized = f.Random.Shuffle(range);
            var unique = randomized.Take(count);
            var enumerator = unique.GetEnumerator();


            var customers = new Faker<Customer>()
                //.RuleFor(x => x.CustomerId, x => idCounter++)
                .RuleFor(x => x.CustomerId, x =>
                {
                    enumerator.MoveNext();
                    return enumerator.Current;
                })
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                .RuleFor(x => x.SecondName, x => x.Person.LastName)
                //.RuleFor(x=>x.PhoneNumber,x=>new Random().Next(000000000,99999999))                
                .RuleFor(x => x.PhoneNumber, (x) => int.Parse(x.Random.Replace("#########")))
                .Generate(count);
            return customers;
        }


    }
}
