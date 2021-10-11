﻿using Bogus;
using Kontrer.OwnerClient.Application.Customers;
using Kontrer.OwnerClient.Application.Orders;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm
{
    public static class DataSeeder
    {
        public static int NumberOfCustomers = 5;
        public static int NumberOfOrders = 5;

        public static async Task SeedData(IServiceProvider services)
        {
            var randomizer = new Random();
            var customerMana = services.GetRequiredService<ICustomerManager>();
            var customers = await customerMana.GetCustomers();
            foreach (var customer in customers.ToList())
            {
                await customerMana.DeleteCustomer(customer.Id);
            }
            var faker = new Faker();
            for (int customerIndex = 0; customerIndex < NumberOfCustomers; customerIndex++)
            {
                var newCustomer = await customerMana.CreateCustomer(faker.Name.FirstName(), faker.Name.LastName(), faker.Internet.Email());
            }

            var orderMana = services.GetRequiredService<IOrderManager>();
            var orders = await orderMana.GetOrders();

            foreach (var order in orders.ToList())
            {
                await orderMana.DeleteOrder(order.Order.Id);
            }

            customers = await customerMana.GetCustomers();
            for (int orderIndex = 0; orderIndex < NumberOfOrders; orderIndex++)
            {
                var newOrder = await orderMana.CreateOrder(customers[randomizer.Next(0, customers.Count)].Id);
                if (randomizer.Next(0, 2) == 1)
                {
                    await orderMana.Process(newOrder.Order);
                }
            }
        }
    }
}