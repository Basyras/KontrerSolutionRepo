using System;
using System.ComponentModel.DataAnnotations;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework
{
    [Obsolete("Use domain entity")]
    public class EFAccommodationOrderEntity
    {
        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
    }
}