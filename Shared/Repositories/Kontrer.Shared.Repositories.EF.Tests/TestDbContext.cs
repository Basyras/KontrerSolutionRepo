using Kontrer.OwnerServer.Shared.Data.EF.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories.EF.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<PersonEntity> People { get; set; }
        public DbSet<CarEntity> Cars { get; set; }
    }
}