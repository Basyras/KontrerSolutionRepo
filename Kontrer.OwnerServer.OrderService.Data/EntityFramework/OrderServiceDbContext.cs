using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Data.EntityFramework
{
    public class OrderServiceDbContext:DbContext
    {
        public List<AccommodationOrderEntity> Orders { get; set; }
    }
}
