﻿using Kontrer.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.EntityFramework
{
    public class OwnerServerDbContext : DbContext
    {
        public virtual DbSet<AccommodationEntity> Accommodations { get; set; }
        public virtual DbSet<PriceSettingEntity> PriceSettings { get; set; }
    }
}