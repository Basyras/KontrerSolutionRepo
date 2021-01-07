using System;

namespace Kontrer.OwnerServer.OrderService.Shared
{
    public class NewOrderModel
    {
        public int CustomerId { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int OrderId { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }

        
    }
}
