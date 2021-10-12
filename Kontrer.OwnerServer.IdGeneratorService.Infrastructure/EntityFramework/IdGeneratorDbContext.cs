using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Infrastructure.EntityFramework
{
    public class IdGeneratorDbContext : DbContext, IDesignTimeDbContextFactory<IdGeneratorDbContext>

    {
        public IdGeneratorDbContext()
        {
        }

        public IdGeneratorDbContext(DbContextOptions<IdGeneratorDbContext> options) : base(options)
        {
        }

        public virtual DbSet<LastUsedIdEntity> LastUsedIds { get; set; }

        public IdGeneratorDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            DbContextOptionsBuilder<IdGeneratorDbContext> builder = new DbContextOptionsBuilder<IdGeneratorDbContext>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new IdGeneratorDbContext(builder.Options);
        }
    }
}