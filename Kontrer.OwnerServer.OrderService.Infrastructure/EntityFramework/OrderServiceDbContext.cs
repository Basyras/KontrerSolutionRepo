using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework
{
    public class OrderServiceDbContext : DbContext, IDesignTimeDbContextFactory<OrderServiceDbContext>
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=OrderServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : base(options)
        {
        }

        public OrderServiceDbContext()
        {
        }

        public DbSet<AccommodationOrderEntity> Orders { get; set; }

        public OrderServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderServiceDbContext>();
            optionsBuilder.UseSqlServer(debugConnectionString);

            return new OrderServiceDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AccommodationOrderEntity>()
            //    .OwnsOne(x => x.Requirment)
            //    .OwnsMany(x => x.Rooms)
            //    .OwnsMany(x => x.People)
            //    .OwnsMany(x => x.PersonItems);

            //modelBuilder.Entity<AccommodationOrderEntity>()
            //    .OwnsOne(x => x.Requirment)
            //    .OwnsMany(x => x.Rooms)
            //    .OwnsMany(x => x.RoomItems);

            //modelBuilder.Entity<AccommodationOrderEntity>()
            //    .OwnsOne(x => x.Requirment)
            //    .OwnsMany(x => x.AccommodationItems);
            ///
            /// ///
            ///
            //var requirementOwned = modelBuilder.Entity<AccommodationOrderEntity>()
            // .OwnsOne(x => x.Requirment);

            //requirementOwned.OwnsMany(x => x.Discounts);
            //var accoItem = requirementOwned.OwnsMany(x => x.AccommodationItems);
            //accoItem.OwnsMany(x => x.Discounts);
            //accoItem.OwnsOne(x => x.CostPerOne);

            //var roomOwned = requirementOwned.OwnsMany(x => x.Rooms);
            //var roomItemOwned = roomOwned.OwnsMany(x => x.RoomItems);
            //roomOwned.OwnsMany(x => x.Discounts);
            //roomItemOwned.OwnsMany(x => x.Discounts);

            //var personOwned = roomOwned.OwnsMany(x => x.People);

            //var personItemOwned = personOwned.OwnsMany(x => x.PersonItems);
            //;
            //personOwned.OwnsMany(x => x.Discounts);
            //personItemOwned.OwnsMany(x => x.Discounts);
            ///
            ////
            /////
            /////
            modelBuilder.Entity<AccommodationOrderEntity>()
            .OwnsOne(order => order.Requirment, requirement =>
            {
                requirement.OwnsMany(x => x.AccommodationItems, accoItem =>
                {
                    accoItem.OwnsMany(x => x.Discounts, discount =>
                    {
                        discount.OwnsOne(x => x.AmountDiscount);
                    });
                    accoItem.OwnsOne(x => x.CostPerOne);
                });

                requirement.OwnsMany(x => x.Discounts, discount =>
                {
                    discount.OwnsOne(x => x.AmountDiscount);
                });
                requirement.OwnsMany(x => x.Rooms, room =>
                {
                    room.OwnsMany(x => x.People, person =>
                    {
                        person.OwnsMany(x => x.PersonItems, personItem =>
                        {
                            personItem.OwnsMany(x => x.Discounts, discount =>
                            {
                                discount.OwnsOne(x => x.AmountDiscount);
                            });
                            personItem.OwnsOne(x => x.CostPerOne);
                        });
                        person.OwnsMany(x => x.Discounts, discount =>
                        {
                            discount.OwnsOne(x => x.AmountDiscount);
                        });
                    });
                    room.OwnsMany(x => x.Discounts, discount =>
                    {
                        discount.OwnsOne(x => x.AmountDiscount);
                    });
                    room.OwnsMany(x => x.RoomItems, personItem =>
                    {
                        personItem.OwnsMany(x => x.Discounts, discount =>
                        {
                            discount.OwnsOne(x => x.AmountDiscount);
                        });
                        personItem.OwnsOne(x => x.CostPerOne);
                    });
                });
            });

            modelBuilder.Entity<AccommodationOrderEntity>()
                .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}