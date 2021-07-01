using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrders;
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
            //Write Fluent API configurations here

            // Property Configurations
            modelBuilder.Entity<AccommodationOrderEntity>().OwnsOne(x => x.Requirment).OwnsMany(x => x.Rooms);
            modelBuilder.Entity<AccommodationOrderEntity>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}