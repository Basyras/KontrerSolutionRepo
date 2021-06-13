namespace Kontrer.OwnerServer.OrderService.Infrastructure.EntityFramework
{
    public class AccommodationOrderEntity
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }

        

    }
}
