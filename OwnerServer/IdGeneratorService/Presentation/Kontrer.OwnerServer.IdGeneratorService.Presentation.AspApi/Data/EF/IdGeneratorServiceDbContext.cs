using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Data.EF
{
    public class IdGeneratorServiceDbContext : DbContext, IDesignTimeDbContextFactory<IdGeneratorServiceDbContext>
    
    {

        public IdGeneratorServiceDbContext()
        {

        }
        public IdGeneratorServiceDbContext(DbContextOptions<IdGeneratorServiceDbContext> options) : base (options)
        {

        }
    
        
        public virtual DbSet<LastUsedIdEntity> LastUsedIds { get; set; }

        public IdGeneratorServiceDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            DbContextOptionsBuilder<IdGeneratorServiceDbContext> builder = new DbContextOptionsBuilder<IdGeneratorServiceDbContext>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new IdGeneratorServiceDbContext(builder.Options);
        }
    }
}
