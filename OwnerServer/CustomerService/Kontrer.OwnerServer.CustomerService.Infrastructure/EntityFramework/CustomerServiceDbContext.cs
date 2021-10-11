using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework
{
    public class CustomerServiceDbContext : DbContext, IDesignTimeDbContextFactory<CustomerServiceDbContext>
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : base(options)
        {
        }

        public CustomerServiceDbContext()
        {
        }

        public DbSet<CustomerEntity> Customers { get; set; }

        public CustomerServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerServiceDbContext>();
            optionsBuilder.UseSqlServer(debugConnectionString);

            return new CustomerServiceDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>()
                .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}